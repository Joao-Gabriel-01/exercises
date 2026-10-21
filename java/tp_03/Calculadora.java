import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.*;

/**
   Exercício 1 - Calculadora
   Matéria: CBTLPR2 (Java) – ADS 471 – Professor: Wellington Tuler Moraes
   Alunos: João Gabriel da Silva e Guilherme Bertero

  Requisitos:
  - Operações: divisão (/), multiplicação (*), subtração (-) e adição (+);
  - Botão C = Clear: limpa a memória da calculadora e zera o text field de resultado;
  - Tratamento de erros com bloco (try, catch, finally).
 */
public class Calculadora extends JFrame implements ActionListener {

    private JTextField txtVisor;

    private double memoria = 0;
    private String operacao = "";
    private boolean novoNumero = true;

    public Calculadora() {
        super("Calcul...");
        initComponents();
    }

    private void initComponents() {
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(240, 280);
        setLocationRelativeTo(null);
        setLayout(new BorderLayout(5, 5));


        txtVisor = new JTextField("0");
        txtVisor.setHorizontalAlignment(JTextField.RIGHT);
        txtVisor.setEditable(false);
        txtVisor.setFont(new Font("Arial", Font.BOLD, 18));
        txtVisor.setBackground(Color.WHITE);
        txtVisor.setBorder(BorderFactory.createCompoundBorder(
                BorderFactory.createLineBorder(Color.GRAY),
                BorderFactory.createEmptyBorder(5, 5, 5, 5)
        ));

        JPanel painelVisor = new JPanel(new BorderLayout());
        painelVisor.setBorder(BorderFactory.createEmptyBorder(10, 10, 5, 10));
        painelVisor.add(txtVisor, BorderLayout.CENTER);


        JPanel painelBotoes = new JPanel(new GridLayout(5, 4, 5, 5));
        painelBotoes.setBorder(BorderFactory.createEmptyBorder(5, 10, 10, 10));


        painelBotoes.add(criarBotao("7"));
        painelBotoes.add(criarBotao("8"));
        painelBotoes.add(criarBotao("9"));
        painelBotoes.add(criarBotao("/"));


        painelBotoes.add(criarBotao("4"));
        painelBotoes.add(criarBotao("5"));
        painelBotoes.add(criarBotao("6"));
        painelBotoes.add(criarBotao("*"));


        painelBotoes.add(criarBotao("1"));
        painelBotoes.add(criarBotao("2"));
        painelBotoes.add(criarBotao("3"));
        painelBotoes.add(criarBotao("-"));


        painelBotoes.add(criarBotao("0"));
        painelBotoes.add(criarBotao("."));
        painelBotoes.add(criarBotao("="));
        painelBotoes.add(criarBotao("+"));


        painelBotoes.add(criarBotao("C"));
        painelBotoes.add(new JLabel(""));
        painelBotoes.add(new JLabel(""));
        painelBotoes.add(new JLabel(""));

        add(painelVisor, BorderLayout.NORTH);
        add(painelBotoes, BorderLayout.CENTER);
    }

    private JButton criarBotao(String texto) {
        JButton btn = new JButton(texto);
        btn.setFont(new Font("Arial", Font.BOLD, 14));
        btn.setFocusPainted(false);
        btn.addActionListener(this);
        return btn;
    }

    @Override
    public void actionPerformed(ActionEvent e) {
        String comando = e.getActionCommand();

        if (comando.matches("[0-9]")) {
            processarDigito(comando);
        } else if (comando.equals(".")) {
            processarPonto();
        } else if (comando.equals("+") || comando.equals("-") || comando.equals("*") || comando.equals("/")) {
            processarOperador(comando);
        } else if (comando.equals("=")) {
            processarIgual();
        } else if (comando.equals("C")) {
            limparMemoria();
        }
    }

    private void processarDigito(String digito) {
        if (novoNumero || txtVisor.getText().equals("0") || txtVisor.getText().equals("Erro")) {
            txtVisor.setText(digito);
            novoNumero = false;
        } else {
            txtVisor.setText(txtVisor.getText() + digito);
        }
    }

    private void processarPonto() {
        if (novoNumero || txtVisor.getText().equals("Erro")) {
            txtVisor.setText("0.");
            novoNumero = false;
        } else if (!txtVisor.getText().contains(".")) {
            txtVisor.setText(txtVisor.getText() + ".");
        }
    }

    private void processarOperador(String op) {
        if (!operacao.isEmpty() && !novoNumero) {
            calcular();
        }
        try {
            memoria = Double.parseDouble(txtVisor.getText());
            operacao = op;
            novoNumero = true;
        } catch (NumberFormatException ex) {
            txtVisor.setText("Erro");
            memoria = 0;
            operacao = "";
        }
    }

    private void processarIgual() {
        if (!operacao.isEmpty()) {
            calcular();
        }
    }

    private void calcular() {
        try {
            double valor2 = Double.parseDouble(txtVisor.getText());
            double resultado = 0;

            switch (operacao) {
                case "+":
                    resultado = memoria + valor2;
                    break;
                case "-":
                    resultado = memoria - valor2;
                    break;
                case "*":
                    resultado = memoria * valor2;
                    break;
                case "/":
                    if (valor2 == 0) {
                        throw new ArithmeticException("Divisão por zero não é permitida!");
                    }
                    resultado = memoria / valor2;
                    break;
                default:
                    resultado = valor2;
                    break;
            }

            if (resultado == (long) resultado) {
                txtVisor.setText(String.valueOf((long) resultado));
            } else {
                txtVisor.setText(String.valueOf(resultado));
            }
            memoria = resultado;

        } catch (ArithmeticException ex) {
            JOptionPane.showMessageDialog(this, ex.getMessage(), "Erro de Cálculo", JOptionPane.ERROR_MESSAGE);
            txtVisor.setText("Erro");
            memoria = 0;
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(this, "Número com formato inválido!", "Erro de Formato", JOptionPane.ERROR_MESSAGE);
            txtVisor.setText("Erro");
            memoria = 0;
        } catch (Exception ex) {
            JOptionPane.showMessageDialog(this, "Erro inesperado: " + ex.getMessage(), "Erro Geral", JOptionPane.ERROR_MESSAGE);
            txtVisor.setText("Erro");
            memoria = 0;
        } finally {
            operacao = "";
            novoNumero = true;
        }
    }

    private void limparMemoria() {
        memoria = 0;
        operacao = "";
        novoNumero = true;
        txtVisor.setText("0");
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            new Calculadora().setVisible(true);
        });
    }
}
