import java.util.Scanner;
import java.util.Date;
import java.text.DateFormat;

//João Gabriel da Silva e Guilherme Bertero
// Exercício 01: Reescreva as propriedades e métodos da classe Data, deixando-os de acordo como padrão UML (Getter e Setter).
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
        setDia();
        setMes();
        setAno();
    }

    
    public Data(int d, int m, int a) {
        this.dia = d;
        this.mes = m;
        this.ano = a;
    }

    
    public void setDia(int d) {
        this.dia = d;
    }

    public void setMes(int m) {
        this.mes = m;
    }

    public void setAno(int a) {
        this.ano = a;
    }

    
    public void setDia() {
        Scanner sc = new Scanner(System.in);
        boolean valido = false;
        while (!valido) {
            try {
                System.out.print("Digite o dia: ");
                int d = Integer.parseInt(sc.nextLine().trim());
                if (d >= 1 && d <= 31) {
                    this.dia = d;
                    valido = true;
                } else {
                    System.out.println("Dia inválido! Deve estar entre 1 e 31.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Valor inválido! Digite um número inteiro.");
            }
        }
    }

    public void setMes() {
        Scanner sc = new Scanner(System.in);
        boolean valido = false;
        while (!valido) {
            try {
                System.out.print("Digite o mês (1-12): ");
                int m = Integer.parseInt(sc.nextLine().trim());
                if (m >= 1 && m <= 12) {
                    this.mes = m;
                    valido = true;
                } else {
                    System.out.println("Mês inválido! Deve estar entre 1 e 12.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Valor inválido! Digite um número inteiro.");
            }
        }
    }

    public void setAno() {
        Scanner sc = new Scanner(System.in);
        boolean valido = false;
        while (!valido) {
            try {
                System.out.print("Digite o ano (ex: 2025): ");
                int a = Integer.parseInt(sc.nextLine().trim());
                if (a >= 1) {
                    this.ano = a;
                    valido = true;
                } else {
                    System.out.println("Ano inválido! Deve ser maior que zero.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Valor inválido! Digite um número inteiro.");
            }
        }
    }

    
    public int getDia() {
        return dia;
    }

    public int getMes() {
        return mes;
    }

    public int getAno() {
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
