import java.util.Scanner;
import java.util.Date;
import java.text.DateFormat;

//Nome: João Gabriel da Silva e Guilherme Bertero

/*TRABALHO PRÁTICO 04: Construir a classe Data, conforme especificação
abaixo, este exercício comporá a avaliação final, portanto será obrigatória sua
elaboração para a prova final.
Crie a classe Data conforme especificado abaixo:

Data
- dia: int
- mes: int
- ano: int
+ Data()
+ Data(int d, int m, int a)
+ entraDia(int d)
+ entraMes(int m)
+ entraAno(int a)
+ entraDia()
+ entraMes()
+ entraAno()
+ retDia(): int
+ retMes(): int
+ retAno(): int
+ mostra1(): String
+ mostra2(): String
+ bissexto(): boolean
+ diasTranscorridos(): int
+ apresentaDataAtual ():void

• O construtor Data() deverá permitir ao usuário digitar os valores de dia, mês e ano e com eles
inicializar os atributos da classe. Os valores digitados deverão ser consistidos e só aceitos se válidos,
caso contrário, redigitar;
• O construtor Data(int d, int m, int a) deverá receber os valores de dia, mês e ano e com eles inicializar
as propriedades da classe;
• Os métodos entraDia(int d), entraMes(int m) e entraAno(int a) devem receber um valor e atribuí-lo às
respectivas propriedades;

• Os métodos entraDia (),entraMes () e entraAno () devem permitir ao usuário digitar um valor e atribuí-
lo a respectiva propriedade. Os valores digitados devem sofrer consistência e só aceitos quando válidos,

caso contrário, solicitar ao usuário redigitar;
• Os métodos retDia(), retMes() e retAno() devem nos devolver as respectivas propriedades;
• O método mostra1() deve nos devolver a data no formato: dd/mm/aaaa;
• O método mostra2() deve nos devolver a data no formato: dd/mesPorExtenso/ano;
• O método bissexto() deve nos devolver um boolean informando se o ano é ou não bissexto;
• O método diasTranscorridos, deve retornar a quantidade de dias transcorridos no ano até a data
digitada.
• O método apresentaDataAtual() deve imprimir a data atual, utilizando as classes Date e DateFormat, o
DateFormat empregando o seguinte método: getDateInstance(DateFormat.FULL);
• Conveniente colocar tratamento de exceção para as possíveis inconsistências na entrada de dados. */


public class Data {

    private int dia;
    private int mes;
    private int ano;

    private static final String[] MESES = {
        "", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
        "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
    };

    
    private static final int[] DIAS_MES = {
        0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
    };

   
    public Data() {
        entraDia();
        entraMes();
        entraAno();
    }

    public Data(int d, int m, int a) {
        dia = d;
        mes = m;
        ano = a;
    }

  
    public void entraDia(int d) {
        dia = d;
    }

    public void entraMes(int m) {
        mes = m;
    }

    public void entraAno(int a) {
        ano = a;
    }

 
    public void entraDia() {
        Scanner sc = new Scanner(System.in);
        int d = 0;
        boolean valido = false;
        while (!valido) {
            try {
                System.out.print("Digite o dia: ");
                d = Integer.parseInt(sc.nextLine().trim());
                if (d >= 1 && d <= 31) {
                    valido = true;
                } else {
                    System.out.println("Dia inválido! Deve estar entre 1 e 31.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Valor inválido! Digite um número inteiro.");
            }
        }
        dia = d;
    }

    public void entraMes() {
        Scanner sc = new Scanner(System.in);
        int m = 0;
        boolean valido = false;
        while (!valido) {
            try {
                System.out.print("Digite o mês (1-12): ");
                m = Integer.parseInt(sc.nextLine().trim());
                if (m >= 1 && m <= 12) {
                    valido = true;
                } else {
                    System.out.println("Mês inválido! Deve estar entre 1 e 12.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Valor inválido! Digite um número inteiro.");
            }
        }
        mes = m;
    }

    public void entraAno() {
        Scanner sc = new Scanner(System.in);
        int a = 0;
        boolean valido = false;
        while (!valido) {
            try {
                System.out.print("Digite o ano (ex: 2024): ");
                a = Integer.parseInt(sc.nextLine().trim());
                if (a >= 1) {
                    valido = true;
                } else {
                    System.out.println("Ano inválido! Deve ser maior que zero.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Valor inválido! Digite um número inteiro.");
            }
        }
        ano = a;
    }


    public int retDia() {
        return dia;
    }

    public int retMes() {
        return mes;
    }

    public int retAno() {
        return ano;
    }

    public String mostra1() {
        return String.format("%02d/%02d/%04d", dia, mes, ano);
    }


    public String mostra2() {
        String nomeMes = (mes >= 1 && mes <= 12) ? MESES[mes] : "Inválido";
        return String.format("%02d/%s/%04d", dia, nomeMes, ano);
    }


    public boolean bissexto() {
        return (ano % 4 == 0 && ano % 100 != 0) || (ano % 400 == 0);
    }

    public int diasTranscorridos() {
        int total = 0;

        for (int m = 1; m < mes; m++) {
            if (m == 2 && bissexto()) {
                total += 29;
            } else {
                total += DIAS_MES[m];
            }
        }

        total += dia;

        return total;
    }

    public void apresentaDataAtual() {
        Date hoje = new Date();
        DateFormat df = DateFormat.getDateInstance(DateFormat.FULL);
        System.out.println("Data atual: " + df.format(hoje));
    }
}
