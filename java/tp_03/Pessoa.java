/**
  Exercício 2 - Estrutura de Classes indicada no PDF
  Matéria: CBTLPR2 (Java) – ADS 471 – Professor: Wellington Tuler Moraes
  Alunos: João Gabriel da Silva e Guilherme Bertero
 */
public class Pessoa {


    protected static int kp = 0;
    protected String nome;
    protected char sexo;
    protected int idade;


    public Pessoa() {
        this.nome = "";
        this.sexo = ' ';
        this.idade = 0;
    }


    public Pessoa(String nome, char sexo, int idade) {
        this.nome = nome;
        this.sexo = sexo;
        this.idade = idade;
    }


    public static void setKp() {
        kp++;
    }

    public static int getKp() {
        return kp;
    }

    public void setNome(String nome) {
        this.nome = nome;
    }

    public String getNome() {
        return this.nome;
    }

    public void setSexo(char sexo) {
        this.sexo = sexo;
    }

    public char getSexo() {
        return this.sexo;
    }

    public void setIdade(int idade) {
        this.idade = idade;
    }

    public int getIdade() {
        return this.idade;
    }
}
