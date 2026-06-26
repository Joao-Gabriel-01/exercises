import java.io.FileWriter;
import java.io.PrintWriter;
import java.io.IOException;

//João Gabriel da Silva e Guilherme Bertero

/*Exercício 03: Para testar a classe criada siga os passos abaixo respeitando a ordem em que eles
serão solicitados:
• Usando a classe ConsultaAgendada instancie o objeto p1 inicializado-o com o
construtor ConsultaAgendada (int h, int mi, int s, int d, int m, int a, String p, String
m);
• Exiba todas as propriedades de p1;
• Agora instancie o objeto p2 usando o construtor ConsultaAgendada ();
• Exiba todas as propriedades de p2;
• Usando os métodos setData(), setHora(), setNomePaciente(), setNomeMedico altere
as propriedades de p1;
• Exiba todas as propriedades de p1 novamente.
• Exiba a quantidade final de consultas.*/
public class Main {

    public static void main(String[] args) {

        StringBuilder saida = new StringBuilder();

       
        System.out.println("========================================");
        System.out.println(" Criando p1 via construtor com parâmetros");
        System.out.println("========================================");
        ConsultaAgendada p1 = new ConsultaAgendada(
                14, 30, 0,  
                25, 6, 2025,    
                "Ana Paula",    
                "Dr. Carlos"    
        );

        System.out.println("\n--- Propriedades de p1 ---");
        System.out.println(p1);
        saida.append("--- Propriedades de p1 (inicial) ---\n");
        saida.append(p1).append("\n");

        System.out.println("\n========================================");
        System.out.println(" Criando p2 via construtor padrão (teclado)");
        System.out.println("========================================");
        ConsultaAgendada p2 = new ConsultaAgendada();

        System.out.println("\n--- Propriedades de p2 ---");
        System.out.println(p2);
        saida.append("\n--- Propriedades de p2 ---\n");
        saida.append(p2).append("\n");

        System.out.println("\n========================================");
        System.out.println(" Alterando propriedades de p1 via setters");
        System.out.println("========================================");
        p1.setData();
        p1.setHora();
        p1.setNomePaciente();
        p1.setNomeMedico();

        System.out.println("\n--- Propriedades de p1 (após alteração) ---");
        System.out.println(p1);
        saida.append("\n--- Propriedades de p1 (após alteração) ---\n");
        saida.append(p1).append("\n");

        String qtd = "\nQuantidade total de consultas agendadas: " + ConsultaAgendada.getAmostra();
        System.out.println(qtd);
        saida.append(qtd).append("\n");

        // Exercício 04: Escreva todo o resultado obtido no exercício 3 em um arquivo texto.
        try (PrintWriter pw = new PrintWriter(new FileWriter("resultado.txt"))) {
            System.out.println("Salvando em: " + new java.io.File("resultado.txt").getAbsolutePath());
            pw.println(saida.toString());
            System.out.println("\nResultado gravado em: resultado.txt");
        } catch (IOException e) {
            System.out.println("Erro ao gravar arquivo: " + e.getMessage());
        }
    }
}
