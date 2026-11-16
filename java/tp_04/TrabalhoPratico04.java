/*
  CBTLPR2 - ADS 471 - Professor: Wellington Tuler Moraes
  Trabalho Prático 04
  Aluno: João Gabriel da Silva e Gael Felix

  Enunciado:
  1. Elabore um programa em Java que apresente um frame semelhante ao que
     se segue (campo Nome + botão Pesquisar; rótulos Nome, Salário e Cargo;
     botões Anterior e Próximo).
  2. Deverá criar um banco SQL Server chamado aulajava com as tabelas
     conforme esquema (tbcargos e tbfuncs)  -> ver script_aulajava.sql.
  3. Acrescente alguns registros, respeitando as chaves e o relacionamento;
  4. Estabeleça a conexão utilizando o JDBC;
  5. Ao clicar no botão Pesquisar, deverá ser efetuado o select (utilize like)
     para "preencher" um recordset e PreparedStatement para fazer o SQL.
  6. Os botões Próximo e Anterior devem permitir a navegação pelo recordset
     até os limites inicial e final.
 */

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.FlowLayout;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.math.BigDecimal;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Locale;


import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.SwingUtilities;

public class TrabalhoPratico04 extends JFrame {

    private static final long serialVersionUID = 1L;


    private static final String URL =
            "jdbc:sqlserver://localhost:1433;databaseName=aulajava;"
                    + "encrypt=false;trustServerCertificate=true";
    private static final String USUARIO = "sa";
    private static final String SENHA = "Java2026Tp04";


    private static final String SQL =
            "SELECT f.nome_func, f.sal_func, c.ds_cargo "
            + "FROM tbfuncs f "
            + "LEFT JOIN tbcargos c ON c.cd_cargo = f.cod_cargo "
            + "WHERE f.nome_func LIKE ? "
            + "ORDER BY f.nome_func";

    //JDBC
    private Connection conexao;
    private PreparedStatement stmt;
    private ResultSet rs;
    private int totalRegistros;
    private int posicao;

    private final JTextField txtBusca = new JTextField(22);
    private final JButton btnPesquisar = new JButton("Pesquisar");
    private final JTextField txtNome = criarCampoSomenteLeitura();
    private final JTextField txtSalario = criarCampoSomenteLeitura();
    private final JTextField txtCargo = criarCampoSomenteLeitura();
    private final JButton btnAnterior = new JButton("Anterior");
    private final JButton btnProximo = new JButton("Próximo");

    public TrabalhoPratico04() {
        super("TRABALHO PRATICO 04");
        montarTela();
        conectar();
        atualizarBotoes();
    }

   //interface
    private void montarTela() {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
        setSize(420, 250);
        setLocationRelativeTo(null);
        setLayout(new BorderLayout(0, 8));


        JPanel linhaBusca = new JPanel(new FlowLayout(FlowLayout.CENTER));
        linhaBusca.add(criarRotulo("Nome:"));
        linhaBusca.add(txtBusca);

        JPanel linhaBotao = new JPanel(new FlowLayout(FlowLayout.CENTER));
        linhaBotao.add(btnPesquisar);

        JPanel topo = new JPanel(new GridLayout(2, 1));
        topo.add(linhaBusca);
        topo.add(linhaBotao);
        add(topo, BorderLayout.NORTH);


        JPanel centro = new JPanel(new GridLayout(3, 2, 5, 5));
        centro.setBorder(BorderFactory.createEmptyBorder(5, 0, 0, 0));
        centro.add(criarRotulo("Nome:"));
        centro.add(txtNome);
        centro.add(criarRotulo("Salário:"));
        centro.add(txtSalario);
        centro.add(criarRotulo("Cargo:"));
        centro.add(txtCargo);
        add(centro, BorderLayout.CENTER);


        JPanel rodape = new JPanel(new GridLayout(1, 2));
        rodape.add(btnAnterior);
        rodape.add(btnProximo);
        add(rodape, BorderLayout.SOUTH);


        btnPesquisar.addActionListener(e -> pesquisar());
        txtBusca.addActionListener(e -> pesquisar());
        btnAnterior.addActionListener(e -> anterior());
        btnProximo.addActionListener(e -> proximo());


        addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {
                fecharTudo();
            }
        });
    }

    private static JLabel criarRotulo(String texto) {
        JLabel lbl = new JLabel(texto);
        lbl.setFont(lbl.getFont().deriveFont(Font.BOLD));
        return lbl;
    }

    private static JTextField criarCampoSomenteLeitura() {
        JTextField campo = new JTextField();
        campo.setEditable(false);
        campo.setBackground(Color.WHITE);
        return campo;
    }


    // JDBC
    private void conectar() {
        try {
            conexao = DriverManager.getConnection(URL, USUARIO, SENHA);
        } catch (SQLException ex) {
            JOptionPane.showMessageDialog(this,
                    "Não foi possível conectar ao banco aulajava:\n" + ex.getMessage(),
                    "Erro de conexão", JOptionPane.ERROR_MESSAGE);
            System.exit(1);
        }
    }

    private void pesquisar() {
        String nome = txtBusca.getText().trim();
        try {
            fecharConsulta();
            stmt = conexao.prepareStatement(SQL,
                    ResultSet.TYPE_SCROLL_INSENSITIVE,
                    ResultSet.CONCUR_READ_ONLY);
            stmt.setString(1, "%" + nome + "%");
            rs = stmt.executeQuery();

            rs.last();
            totalRegistros = rs.getRow();

            if (totalRegistros == 0) {
                posicao = 0;
                limparCampos();
                atualizarBotoes();
                JOptionPane.showMessageDialog(this,
                        "Nenhum funcionário encontrado.",
                        "Pesquisa", JOptionPane.INFORMATION_MESSAGE);
                return;
            }

            rs.first();
            posicao = 1;
            mostrarRegistro();
        } catch (SQLException ex) {
            mostrarErro(ex);
        }
    }

    private void proximo() {
        try {
            if (rs != null && posicao < totalRegistros) {
                rs.next();
                posicao++;
                mostrarRegistro();
            }
        } catch (SQLException ex) {
            mostrarErro(ex);
        }
    }

    private void anterior() {
        try {
            if (rs != null && posicao > 1) {
                rs.previous();
                posicao--;
                mostrarRegistro();
            }
        } catch (SQLException ex) {
            mostrarErro(ex);
        }
    }

    private void mostrarRegistro() throws SQLException {
        String nome = rs.getString("nome_func");
        BigDecimal salario = rs.getBigDecimal("sal_func");
        String cargo = rs.getString("ds_cargo");


        txtNome.setText(nome == null ? "" : nome.trim());
        txtSalario.setText(salario == null ? "" : String.format(Locale.US, "%.2f", salario));
        txtCargo.setText(cargo == null ? "" : cargo.trim());

        atualizarBotoes();
    }

    private void limparCampos() {
        txtNome.setText("");
        txtSalario.setText("");
        txtCargo.setText("");
    }

    // Desabilita Anterior no primeiro registro e Próximo no último
    private void atualizarBotoes() {
        btnAnterior.setEnabled(totalRegistros > 0 && posicao > 1);
        btnProximo.setEnabled(totalRegistros > 0 && posicao < totalRegistros);
    }

    private void mostrarErro(SQLException ex) {
        JOptionPane.showMessageDialog(this, "Erro no banco de dados:\n" + ex.getMessage(),
                "Erro", JOptionPane.ERROR_MESSAGE);
    }

    private void fecharConsulta() {
        try {
            if (rs != null) {
                rs.close();
            }
            if (stmt != null) {
                stmt.close();
            }
        } catch (SQLException ignorada) {
            // nada a fazer
        }
        rs = null;
        stmt = null;
    }

    private void fecharTudo() {
        fecharConsulta();
        try {
            if (conexao != null) {
                conexao.close();
            }
        } catch (SQLException ignorada) {
            // nada a fazer
        }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> new TrabalhoPratico04().setVisible(true));
    }
}
