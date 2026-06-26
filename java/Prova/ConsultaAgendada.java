//João Gabriel da Silva e Guilherme Bertero

/*Exercício 02: Crie a classe ConsultaAgendada conforme especificado abaixo (2,0):

ConsultaAgendada

- data: Data
- hora: Hora
- nomePaciente: String
- quantidade: int (static)
- nomeMedico: String
+ ConsultaAgendada ()
+ ConsultaAgendada (int h, int mi, int s, int d, int m, int a, String p,
String m)
+ ConsultaAgendada (Data d, Hora h, String p, String m)
+ setData(int a, int b, int c)
+ setData()
+ setHora(int a, int b, int c)
+ setHora()
+ setNomePaciente(String p)
+ setNomePaciente()
+ setNomeMedico(String m)
+ setNomeMedico()
+ getAmostra(): int
+ getData(): String
+ getHora(): String
+ getNomePaciente (): String
+ getNomeMedico(): String

• O construtor ConsultaAgendada deve nos permitir a digitação dos valores de data,
hora, nome do paciente e do médico;
• Os outros dois construtores devem receber os valores de data, hora, nome do
paciente e do médico sob a forma de parâmetros;
• Qualquer construtor chamado deve acrescer 1 no atributo quantidade.
• Os métodos setData(), setHora(), setNomePaciente(), setNomeMedico() devem nos
permitir alterar os valores das respectivas propriedades através da digitação de
novos valores;
• Os demais métodos “set” devem alterar os valores das propriedades a partir dos
parâmetros recebidos;
• O método getData() deve nos devolver a data no formato: dd/mm/aa;
• O método getHora() deve nos devolver a hora no formato: hh:mm:ss.*/

import java.util.Scanner;

public class ConsultaAgendada {

    private Data data;
    private Hora hora;
    private String nomePaciente;
    private static int quantidade = 0;
    private String nomeMedico;

    public ConsultaAgendada() {
        System.out.println("=== Dados da Data da Consulta ===");
        this.data = new Data();
        System.out.println("=== Dados da Hora da Consulta ===");
        this.hora = new Hora();
        setNomePaciente();
        setNomeMedico();
        quantidade++;
    }

    
    public ConsultaAgendada(int h, int mi, int s, int d, int m, int a, String p, String med) {
        this.hora = new Hora(h, mi, s);
        this.data = new Data(d, m, a);
        this.nomePaciente = p;
        this.nomeMedico = med;
        quantidade++;
    }

  
    public ConsultaAgendada(Data d, Hora h, String p, String med) {
        this.data = d;
        this.hora = h;
        this.nomePaciente = p;
        this.nomeMedico = med;
        quantidade++;
    }

    
    public void setData(int a, int b, int c) {
        this.data = new Data(a, b, c);
    }

    public void setHora(int a, int b, int c) {
        this.hora = new Hora(a, b, c);
    }

    public void setNomePaciente(String p) {
        this.nomePaciente = p;
    }

    public void setNomeMedico(String m) {
        this.nomeMedico = m;
    }


    public void setData() {
        System.out.println("=== Alterando Data da Consulta ===");
        this.data = new Data();
    }

    public void setHora() {
        System.out.println("=== Alterando Hora da Consulta ===");
        this.hora = new Hora();
    }

    public void setNomePaciente() {
        Scanner sc = new Scanner(System.in);
        System.out.print("Digite o nome do paciente: ");
        this.nomePaciente = sc.nextLine().trim();
    }

    public void setNomeMedico() {
        Scanner sc = new Scanner(System.in);
        System.out.print("Digite o nome do médico: ");
        this.nomeMedico = sc.nextLine().trim();
    }


    public static int getAmostra() {
        return quantidade;
    }

   
    public String getData() {
        return String.format("%02d/%02d/%02d",
                data.getDia(),
                data.getMes(),
                data.getAno() % 100);
    }

  
    public String getHora() {
        return hora.mostra();
    }

    public String getNomePaciente() {
        return nomePaciente;
    }

    public String getNomeMedico() {
        return nomeMedico;
    }

    public String toString() {
        return "Paciente : " + nomePaciente  + "\n" +
               "Médico   : " + nomeMedico    + "\n" +
               "Data     : " + getData()     + "\n" +
               "Hora     : " + getHora();
    }
}
