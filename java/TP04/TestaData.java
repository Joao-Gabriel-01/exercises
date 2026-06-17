
//Nome: João Gabriel da Silva e Guilherme Bertero
/*Exercício 02 
Agora, desenvolva um programa capaz de testar a classe e os métodos desenvolvidos no 
exercício anterior.*/


public class TestaData {

    public static void main(String[] args) {

        System.out.println("==============================================");
        System.out.println("  TESTE DA CLASSE DATA - TP04");
        System.out.println("==============================================\n");

        // ------------------------------------------------------------------
        // Teste 1: Construtor com parâmetros Data(int d, int m, int a)
        // ------------------------------------------------------------------
        System.out.println("--- Teste 1: Construtor com parâmetros ---");
        Data dt1 = new Data(17, 6, 2026);
        System.out.println("Data criada: dia=" + dt1.retDia()
                + ", mês=" + dt1.retMes()
                + ", ano=" + dt1.retAno());

        // ------------------------------------------------------------------
        // Teste 2: mostra1() - formato dd/mm/aaaa
        // ------------------------------------------------------------------
        System.out.println("\n--- Teste 2: mostra1() ---");
        System.out.println("Formato dd/mm/aaaa: " + dt1.mostra1());

        // ------------------------------------------------------------------
        // Teste 3: mostra2() - formato dd/mesPorExtenso/ano
        // ------------------------------------------------------------------
        System.out.println("\n--- Teste 3: mostra2() ---");
        System.out.println("Formato dd/mês/ano: " + dt1.mostra2());

        // ------------------------------------------------------------------
        // Teste 4: bissexto()
        // ------------------------------------------------------------------
        System.out.println("\n--- Teste 4: bissexto() ---");
        int[] anos = {2000, 1900, 2024, 2023, 1600};
        for (int a : anos) {
            Data dTeste = new Data(1, 1, a);
            System.out.println("Ano " + a + " é bissexto? " + dTeste.bissexto());
        }

        // ------------------------------------------------------------------
        // Teste 5: diasTranscorridos()
        // ------------------------------------------------------------------
        System.out.println("\n--- Teste 5: diasTranscorridos() ---");

        Data dt2 = new Data(1, 1, 2024);
        System.out.println(dt2.mostra1() + " → dias transcorridos: " + dt2.diasTranscorridos());

        Data dt3 = new Data(31, 12, 2024);
        System.out.println(dt3.mostra1() + " → dias transcorridos: " + dt3.diasTranscorridos()
                + " (2024 é bissexto, esperado 366)");

        Data dt4 = new Data(31, 12, 2023);
        System.out.println(dt4.mostra1() + " → dias transcorridos: " + dt4.diasTranscorridos()
                + " (2023 não é bissexto, esperado 365)");

        Data dt5 = new Data(1, 3, 2024);
        System.out.println(dt5.mostra1() + " → dias transcorridos: " + dt5.diasTranscorridos()
                + " (após fev bissexto, esperado 61)");

        Data dt6 = new Data(1, 3, 2023);
        System.out.println(dt6.mostra1() + " → dias transcorridos: " + dt6.diasTranscorridos()
                + " (após fev normal, esperado 60)");

        // ------------------------------------------------------------------
        // Teste 6: entraDia/entraMes/entraAno com parâmetros
        // ------------------------------------------------------------------
        System.out.println("\n--- Teste 6: setters com parâmetro ---");
        Data dt7 = new Data(1, 1, 2000);
        dt7.entraDia(25);
        dt7.entraMes(12);
        dt7.entraAno(2025);
        System.out.println("Após setters: " + dt7.mostra1());
        System.out.println("Por extenso:  " + dt7.mostra2());

        // ------------------------------------------------------------------
        // Teste 7: Todos os meses por extenso
        // ------------------------------------------------------------------
        System.out.println("\n--- Teste 7: mostra2() para todos os meses ---");
        String[] meses = {"Janeiro","Fevereiro","Março","Abril","Maio","Junho",
                          "Julho","Agosto","Setembro","Outubro","Novembro","Dezembro"};
        for (int m = 1; m <= 12; m++) {
            Data dM = new Data(10, m, 2026);
            System.out.println(dM.mostra2());
        }

        // ------------------------------------------------------------------
        // Teste 8: apresentaDataAtual()
        // ------------------------------------------------------------------
        System.out.println("\n--- Teste 8: apresentaDataAtual() ---");
        dt1.apresentaDataAtual();


        System.out.println("\n==============================================");
        System.out.println("  FIM DOS TESTES");
        System.out.println("==============================================");
    }
}
