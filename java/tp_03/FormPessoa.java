import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.*;

/**
  Exercício 2 - FormPessoa (Versão 01)
  Matéria: CBTLPR2 (Java) – ADS 471 – Professor: Wellington Tuler Moraes
  Alunos: João Gabriel da Silva e Guilherme Bertero

  Características:
  - Campo "Numero" não acessível à digitação;
  - Campos "Nome", "Sexo" e "Idade" obrigatórios;
  - Campo "Sexo" aceita apenas "M" ou "F" (via JTextField);
  - Botão "OK": transfere dados para a instância UmaPessoa e incrementa kp;
  - Botão "Mostrar": descarrega os dados de UmaPessoa e kp nos campos;
  - Botão "Limpar": zera os campos;
  - Botão "Sair": encerra o programa.
 */
public class FormPessoa extends JFrame implements ActionListener {


    private Pessoa UmaPessoa;


    private JTextField txtNumero;
    private JTextField txtNome;
    private JTextField txtSexo;
    private JTextField txtIdade;

    private JButton btnOK;
    private JButton btnLimpar;
    private JButton btnMostrar;
    private JButton btnSair;

    public FormPessoa() {
        super("Semana 06 - Exercicio 01");
        initComponents();
    }

    private void initComponents() {
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(420, 230);
        setLocationRelativeTo(null);
        setLayout(new BorderLayout(10, 10));


        JPanel painelCampos = new JPanel(new GridBagLayout());
        painelCampos.setBorder(BorderFactory.createEmptyBorder(15, 20, 10, 20));
        GridBagConstraints gbc = new GridBagConstraints();
        gbc.fill = GridBagConstraints.HORIZONTAL;
        gbc.insets = new Insets(4, 4, 4, 4);


        JLabel lblNumero = new JLabel("Numero:");
        JLabel lblNome = new JLabel("Nome:");
        JLabel lblSexo = new JLabel("Sexo:");
        JLabel lblIdade = new JLabel("Idade:");

        txtNumero = new JTextField(15);
        txtNumero.setEditable(false);

        txtNome = new JTextField(15);
        txtSexo = new JTextField(15);
        txtIdade = new JTextField(15);


        gbc.gridx = 0; gbc.gridy = 0; gbc.weightx = 0.2;
        painelCampos.add(lblNumero, gbc);
        gbc.gridx = 1; gbc.gridy = 0; gbc.weightx = 0.8;
        painelCampos.add(txtNumero, gbc);


        gbc.gridx = 0; gbc.gridy = 1; gbc.weightx = 0.2;
        painelCampos.add(lblNome, gbc);
        gbc.gridx = 1; gbc.gridy = 1; gbc.weightx = 0.8;
        painelCampos.add(txtNome, gbc);


        gbc.gridx = 0; gbc.gridy = 2; gbc.weightx = 0.2;
        painelCampos.add(lblSexo, gbc);
        gbc.gridx = 1; gbc.gridy = 2; gbc.weightx = 0.8;
        painelCampos.add(txtSexo, gbc);


        gbc.gridx = 0; gbc.gridy = 3; gbc.weightx = 0.2;
        painelCampos.add(lblIdade, gbc);
        gbc.gridx = 1; gbc.gridy = 3; gbc.weightx = 0.8;
        painelCampos.add(txtIdade, gbc);


        JPanel painelBotoes = new JPanel(new FlowLayout(FlowLayout.CENTER, 10, 10));
        btnOK = new JButton("OK");
        btnLimpar = new JButton("Limpar");
        btnMostrar = new JButton("Mostrar");
        btnSair = new JButton("Sair");

        btnOK.addActionListener(this);
        btnLimpar.addActionListener(this);
        btnMostrar.addActionListener(this);
        btnSair.addActionListener(this);

        painelBotoes.add(btnOK);
        painelBotoes.add(btnLimpar);
        painelBotoes.add(btnMostrar);
        painelBotoes.add(btnSair);

        add(painelCampos, BorderLayout.CENTER);
        add(painelBotoes, BorderLayout.SOUTH);
    }

    @Override
    public void actionPerformed(ActionEvent e) {
        if (e.getSource() == btnOK) {
            salvarPessoa();
        } else if (e.getSource() == btnMostrar) {
            mostrarPessoa();
        } else if (e.getSource() == btnLimpar) {
            limparCampos();
        } else if (e.getSource() == btnSair) {
            System.exit(0);
        }
    }

    private void salvarPessoa() {
        String nome = txtNome.getText().trim();
        String sexoStr = txtSexo.getText().trim();
        String idadeStr = txtIdade.getText().trim();

        if (nome.isEmpty()) {
            JOptionPane.showMessageDialog(this, "O campo \"Nome\" é de preenchimento obrigatório!", "Aviso", JOptionPane.WARNING_MESSAGE);
            txtNome.requestFocus();
            return;
        }

        if (sexoStr.isEmpty()) {
            JOptionPane.showMessageDialog(this, "O campo \"Sexo\" é de preenchimento obrigatório!", "Aviso", JOptionPane.WARNING_MESSAGE);
            txtSexo.requestFocus();
            return;
        }

        if (idadeStr.isEmpty()) {
            JOptionPane.showMessageDialog(this, "O campo \"Idade\" é de preenchimento obrigatório!", "Aviso", JOptionPane.WARNING_MESSAGE);
            txtIdade.requestFocus();
            return;
        }

        sexoStr = sexoStr.toUpperCase();
        if (!sexoStr.equals("M") && !sexoStr.equals("F")) {
            JOptionPane.showMessageDialog(this, "O campo \"Sexo\" só deve aceitar \"M\" ou \"F\"!", "Erro", JOptionPane.ERROR_MESSAGE);
            txtSexo.requestFocus();
            return;
        }
        char sexo = sexoStr.charAt(0);


        int idade;
        try {
            idade = Integer.parseInt(idadeStr);
            if (idade < 0) {
                JOptionPane.showMessageDialog(this, "A idade deve ser maior ou igual a zero!", "Erro", JOptionPane.ERROR_MESSAGE);
                txtIdade.requestFocus();
                return;
            }
        } catch (NumberFormatException ex) {
            JOptionPane.showMessageDialog(this, "O campo \"Idade\" deve conter um número inteiro válido!", "Erro", JOptionPane.ERROR_MESSAGE);
            txtIdade.requestFocus();
            return;
        }


        UmaPessoa = new Pessoa(nome, sexo, idade);
        Pessoa.setKp();

        JOptionPane.showMessageDialog(this, "Dados salvos com sucesso no objeto UmaPessoa!", "Sucesso", JOptionPane.INFORMATION_MESSAGE);
    }

    private void mostrarPessoa() {
        if (UmaPessoa == null) {
            JOptionPane.showMessageDialog(this, "Nenhuma pessoa foi cadastrada ainda!", "Aviso", JOptionPane.WARNING_MESSAGE);
            return;
        }

        txtNumero.setText(String.valueOf(Pessoa.getKp()));
        txtNome.setText(UmaPessoa.getNome());
        txtSexo.setText(String.valueOf(UmaPessoa.getSexo()));
        txtIdade.setText(String.valueOf(UmaPessoa.getIdade()));
    }

    private void limparCampos() {
        txtNumero.setText("");
        txtNome.setText("");
        txtSexo.setText("");
        txtIdade.setText("");
        txtNome.requestFocus();
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            new FormPessoa().setVisible(true);
        });
    }
}
