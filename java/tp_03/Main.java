import java.awt.*;
import javax.swing.*;

public class Main extends JFrame {

    public Main() {
        super("Atividades CBTLPR2 (Java) - IFSP Cubatão");
        initComponents();
    }

    private void initComponents() {
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setSize(420, 300);
        setLocationRelativeTo(null);
        setLayout(new BorderLayout(10, 10));

        JLabel lblTitulo = new JLabel("Selecione o exercício a ser executado:", JLabel.CENTER);
        lblTitulo.setFont(new Font("Arial", Font.BOLD, 14));
        lblTitulo.setBorder(BorderFactory.createEmptyBorder(15, 10, 5, 10));
        add(lblTitulo, BorderLayout.NORTH);

        JPanel painelBotoes = new JPanel(new GridLayout(4, 1, 10, 10));
        painelBotoes.setBorder(BorderFactory.createEmptyBorder(10, 30, 20, 30));

        JButton btnCalc = new JButton("1. Calculadora (Try, Catch, Finally)");
        JButton btnV1 = new JButton("2. FormPessoa - Versão 01 (JTextField)");
        JButton btnV2 = new JButton("3. FormPessoa - Versão 02 (JComboBox)");
        JButton btnV3 = new JButton("4. FormPessoa - Versão 03 (JRadioButton)");

        btnCalc.setFont(new Font("Arial", Font.PLAIN, 13));
        btnV1.setFont(new Font("Arial", Font.PLAIN, 13));
        btnV2.setFont(new Font("Arial", Font.PLAIN, 13));
        btnV3.setFont(new Font("Arial", Font.PLAIN, 13));

        btnCalc.addActionListener(e -> new Calculadora().setVisible(true));
        btnV1.addActionListener(e -> new FormPessoa().setVisible(true));
        btnV2.addActionListener(e -> new FormPessoaV2().setVisible(true));
        btnV3.addActionListener(e -> new FormPessoaV3().setVisible(true));

        painelBotoes.add(btnCalc);
        painelBotoes.add(btnV1);
        painelBotoes.add(btnV2);
        painelBotoes.add(btnV3);

        add(painelBotoes, BorderLayout.CENTER);
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> {
            new Main().setVisible(true);
        });
    }
}
