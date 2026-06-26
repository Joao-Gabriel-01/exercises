import java.util.Scanner;

public class Hora {

    private int hora;
    private int minuto;
    private int segundo;

    
    public Hora() {
        setHora();
        setMinuto();
        setSegundo();
    }

    
    public Hora(int h, int mi, int s) {
        this.hora = h;
        this.minuto = mi;
        this.segundo = s;
    }

    
    public void setHora(int h) {
        this.hora = h;
    }

    public void setMinuto(int mi) {
        this.minuto = mi;
    }

    public void setSegundo(int s) {
        this.segundo = s;
    }

    
    public void setHora() {
        Scanner sc = new Scanner(System.in);
        boolean valido = false;
        while (!valido) {
            try {
                System.out.print("Digite a hora (0-23): ");
                int h = Integer.parseInt(sc.nextLine().trim());
                if (h >= 0 && h <= 23) {
                    this.hora = h;
                    valido = true;
                } else {
                    System.out.println("Hora inválida! Deve estar entre 0 e 23.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Valor inválido! Digite um número inteiro.");
            }
        }
    }

    public void setMinuto() {
        Scanner sc = new Scanner(System.in);
        boolean valido = false;
        while (!valido) {
            try {
                System.out.print("Digite os minutos (0-59): ");
                int mi = Integer.parseInt(sc.nextLine().trim());
                if (mi >= 0 && mi <= 59) {
                    this.minuto = mi;
                    valido = true;
                } else {
                    System.out.println("Minuto inválido! Deve estar entre 0 e 59.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Valor inválido! Digite um número inteiro.");
            }
        }
    }

    public void setSegundo() {
        Scanner sc = new Scanner(System.in);
        boolean valido = false;
        while (!valido) {
            try {
                System.out.print("Digite os segundos (0-59): ");
                int s = Integer.parseInt(sc.nextLine().trim());
                if (s >= 0 && s <= 59) {
                    this.segundo = s;
                    valido = true;
                } else {
                    System.out.println("Segundo inválido! Deve estar entre 0 e 59.");
                }
            } catch (NumberFormatException e) {
                System.out.println("Valor inválido! Digite um número inteiro.");
            }
        }
    }

    
    public int getHora() {
        return hora;
    }

    public int getMinuto() {
        return minuto;
    }

    public int getSegundo() {
        return segundo;
    }

    
    public String mostra() {
        return String.format("%02d:%02d:%02d", hora, minuto, segundo);
    }
}
