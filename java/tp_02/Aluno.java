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
 *     a. Criar a seguinte classe Aluno, conforme diagrama:
 *        Aluno
 *          + getEndereco() : String
 *          + getIdade() : int
 *          + getNome() : String
 *          + getUuid() : UUID
 *          + setEndereco(String endereco)
 *          + setIdade(int idade)
 *          + setNome(String nome)
 *          + setUuid(UUID uuid)
 *          - endereco : String
 *          - idade : int
 *          - nome : String
 *          - uuid : UUID
 *
 *     Dica: Pesquisar a classe UUID.
 */

import java.util.UUID;

public class Aluno {

    // Atributos privados (conforme diagrama)
    private String endereco;
    private int idade;
    private String nome;
    private UUID uuid;

    // Getters
    public String getEndereco() {
        return endereco;
    }

    public int getIdade() {
        return idade;
    }

    public String getNome() {
        return nome;
    }

    public UUID getUuid() {
        return uuid;
    }

    // Setters
    public void setEndereco(String endereco) {
        this.endereco = endereco;
    }

    public void setIdade(int idade) {
        this.idade = idade;
    }

    public void setNome(String nome) {
        this.nome = nome;
    }

    public void setUuid(UUID uuid) {
        this.uuid = uuid;
    }
}
