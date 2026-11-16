
IF DB_ID('aulajava') IS NULL
    CREATE DATABASE aulajava;
GO

USE aulajava;
GO


CREATE TABLE tbcargos (
    cd_cargo SMALLINT NOT NULL,
    ds_cargo CHAR(20) NULL,
    CONSTRAINT pk_tbcargos PRIMARY KEY (cd_cargo)
);
GO

CREATE TABLE tbfuncs (
    cod_func  DECIMAL(9,0) NOT NULL,
    nome_func CHAR(30)     NULL,
    sal_func  MONEY        NULL,
    cod_cargo SMALLINT     NULL,
    CONSTRAINT pk_tbfuncs PRIMARY KEY (cod_func),
    CONSTRAINT fk_tbfuncs_tbcargos FOREIGN KEY (cod_cargo)
        REFERENCES tbcargos (cd_cargo)
);
GO


INSERT INTO tbcargos (cd_cargo, ds_cargo) VALUES (1, 'Administrativo');
INSERT INTO tbcargos (cd_cargo, ds_cargo) VALUES (2, 'Analista');
INSERT INTO tbcargos (cd_cargo, ds_cargo) VALUES (3, 'Gerente');
INSERT INTO tbcargos (cd_cargo, ds_cargo) VALUES (4, 'Programador');
INSERT INTO tbcargos (cd_cargo, ds_cargo) VALUES (5, 'Vendedor');
GO

INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (1,  'Marcelo',          2000.00, 1);
INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (2,  'Marcelo Pereira',  3500.00, 2);
INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (3,  'Marcela Souza',    4200.50, 3);
INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (4,  'Marcos Almeida',   3100.00, 4);
INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (5,  'Maria Oliveira',   2800.75, 5);
INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (6,  'Carlos Silva',     3900.00, 4);
INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (7,  'Ana Paula Lima',   2600.00, 1);
INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (8,  'Renato Costa',     5200.00, 3);
INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (9,  'Fernanda Rocha',   3300.00, 2);
INSERT INTO tbfuncs (cod_func, nome_func, sal_func, cod_cargo) VALUES (10, 'Paulo Henrique',   1900.00, 5);
GO


SELECT f.cod_func, f.nome_func, f.sal_func, c.ds_cargo
FROM tbfuncs f
LEFT JOIN tbcargos c ON c.cd_cargo = f.cod_cargo
ORDER BY f.cod_func;
GO
