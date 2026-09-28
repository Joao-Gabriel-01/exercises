/*
 * Aluno: João Gabriel da Silva
 *
 * Instituto Federal de Educação, Ciência e Tecnologia de São Paulo - Campus Cubatão
 * Matéria: CBTLPR2 (Java) - ADS 471 - Prof.: Wellington Tuler Moraes
 * CBTLPR2_TP_02
 *
 * Enunciado:
 * 1 - Desenvolver o seguinte sistema abaixo, empregando os conhecimentos
 *     adquiridos nas aulas sobre interfaces gráficas em java.
 *
 *     a. Criar a classe Aluno, conforme diagrama (ver Aluno.java).
 *
 *     b. Implementar o formulário possibilitando ao usuário cadastrar uma
 *        lista de alunos, respeitando as seguintes regras:
 *          i.   Quando o botão Ok for pressionado os dados contidos em tela
 *               devem ser armazenados em memória (utilizar a interface
 *               List<E> em conjunto com a classe ArrayList<E> para armazenar
 *               a lista de alunos cadastrados em memória).
 *          ii.  Limpar apaga o conteúdo dos labels.
 *          iii. Botão Mostrar exibe o pop-up a ser desenvolvido no item (c)
 *               deste trabalho.
 *          iv.  Botão Sair encerra a aplicação.
 *
 *        Layout do formulário (400 x 180):
 *          - Painel superior com GridLayout (3x2) e hgap e vgap 10
 *          - Painel inferior com 4 botões com formatação GridLayout
 *          - Qual Gerenciador de Layout usar para posicionar 2 painéis?
 *            R: BorderLayout (painel superior em NORTH e inferior em SOUTH).
 *
 *     c. Criar o mecanismo de exibição abaixo, que contempla todos os ids e
 *        nomes dos alunos cadastrados nesta execução do programa, utilizando
 *        a classe: JOptionPane.showMessageDialog(this, mensagem);
 *
 *          Mensagem
 *          Resultado
 *          Id: 103a3e47-b447-4c90-ad4f-c04f00f46b8d  Nome: wellington
 *          Id: 69738373-6e73-4b20-84d8-b52ae97edecb  Nome: Maria
 */

import java.awt.BorderLayout;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JTextField;

public class TP02 extends JFrame implements ActionListener {

    // Lista de alunos cadastrados em memória (item b.i)
    private List<Aluno> alunos = new ArrayList<Aluno>();

    // Campos de entrada
    private JTextField txtNome = new JTextField();
    private JTextField txtIdade = new JTextField();
    private JTextField txtEndereco = new JTextField();

    // Botões
    private JButton btnOk = new JButton("Ok");
    private JButton btnLimpar = new JButton("Limpar");
    private JButton btnMostrar = new JButton("Mostrar");
    private JButton btnSair = new JButton("Sair");

    public TP02() {
        super("TP02 - LP2I4");

        // Painel superior: GridLayout 3x2 com hgap e vgap 10
        JPanel painelSuperior = new JPanel(new GridLayout(3, 2, 10, 10));
        painelSuperior.add(new JLabel("Nome:"));
        painelSuperior.add(txtNome);
        painelSuperior.add(new JLabel("Idade:"));
        painelSuperior.add(txtIdade);
        painelSuperior.add(new JLabel("Endereço:"));
        painelSuperior.add(txtEndereco);

        // Painel inferior: 4 botões com GridLayout
        JPanel painelInferior = new JPanel(new GridLayout(1, 4));
        painelInferior.add(btnOk);
        painelInferior.add(btnLimpar);
        painelInferior.add(btnMostrar);
        painelInferior.add(btnSair);

        // Registro dos listeners
        btnOk.addActionListener(this);
        btnLimpar.addActionListener(this);
        btnMostrar.addActionListener(this);
        btnSair.addActionListener(this);

        // BorderLayout para posicionar os 2 painéis
        setLayout(new BorderLayout());
        add(painelSuperior, BorderLayout.NORTH);
        add(painelInferior, BorderLayout.SOUTH);

        // Configurações da janela (400 x 180)
        setSize(400, 180);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setVisible(true);
    }

    // Tratamento dos eventos dos botões
    public void actionPerformed(ActionEvent e) {
        if (e.getSource() == btnOk) {
            cadastrar();
        } else if (e.getSource() == btnLimpar) {
            limpar();
        } else if (e.getSource() == btnMostrar) {
            mostrar();
        } else if (e.getSource() == btnSair) {
            System.exit(0);
        }
    }

    // Item b.i: armazena os dados da tela em memória
    private void cadastrar() {
        try {
            Aluno aluno = new Aluno();
            aluno.setUuid(UUID.randomUUID());
            aluno.setNome(txtNome.getText());
            aluno.setIdade(Integer.parseInt(txtIdade.getText().trim()));
            aluno.setEndereco(txtEndereco.getText());
            alunos.add(aluno);
        } catch (NumberFormatException erro) {
            JOptionPane.showMessageDialog(this, "Idade inválida. Digite um número inteiro.",
                    "ERRO", JOptionPane.ERROR_MESSAGE);
        }
    }

    // Item b.ii: apaga o conteúdo dos campos
    private void limpar() {
        txtNome.setText("");
        txtIdade.setText("");
        txtEndereco.setText("");
    }

    // Item b.iii e c: exibe o pop-up com ids e nomes dos alunos cadastrados
    private void mostrar() {
        String mensagem = "Resultado";
        for (Aluno a : alunos) {
            mensagem += "\nId: " + a.getUuid() + "  Nome: " + a.getNome();
        }
        JOptionPane.showMessageDialog(this, mensagem);
    }

    public static void main(String[] args) {
        new TP02();
    }
}
