create database btp;

create table uuser(
    idUser AS ('u' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    name varchar(100),
    passWord varchar(150),
    admin int --0:no, 1:yes
);
insert into uuser (name, passWord, admin)
values
    ('Rakoto', '123', 0),
    ('Rabe', '123', 0),
    ('Admin', 'admin', 1);
alter table uuser add num varchar(50);
update uuser set num = '0340735910' where idUser = 'u1';
update uuser set num = '0340735917' where idUser = 'u2';
update uuser set num = '0340000000' where idUser = 'u3';

create table travaux(
    idTrav AS ('trav' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    num varchar(50),
    designation varchar(100)
);
insert into travaux (num, designation)
values
    ('000', 'TRAVAUX PREPARATIORE'),
    ('100', 'TRAVAUX DE TERRASSEMENT'),
    ('200', 'TRAVAUX EN INFRASTRUCTURE');

create table tache(
    idTache AS ('tache' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    num varchar(50),
    designation varchar(100),
    unite varchar(10),
    pu float,
    idTrav varchar(14) references travaux(idTrav) on delete cascade
);

ALTER TABLE tache NOCHECK CONSTRAINT FK__tache__idTrav__3E52440B;

ALTER TABLE tache WITH CHECK CHECK CONSTRAINT FK__tache__idTrav__3E52440B;

alter table tache add code_travaux varchar(10);
alter table tache add type_travaux varchar(50);
update tache set code_travaux = '000' where idTrav = 'trav1';
update tache set code_travaux = '100' where idTrav = 'trav2';
update tache set code_travaux = '200' where idTrav = 'trav3';
update tache set type_travaux = 'TRAVAUX PREPARATIORE' where idTrav = 'trav1';
update tache set type_travaux = 'TRAVAUX DE TERRASSEMENT' where idTrav = 'trav2';
update tache set type_travaux = 'TRAVAUX EN INFRASTRUCTURE' where idTrav = 'trav3';
ALTER TABLE tache ALTER COLUMN travaux VARCHAR(100);

insert into tache (num, designation, unite, pu, idTrav)
values
    ('001', 'Mur de soutenement et demi Cloture ht 1m', 'm3', 190000.00, 'trav1');
insert into tache (num, designation, unite, pu, idTrav)
values
    ('101', 'Décapage des terrains meubles', 'm2', 3072.87, 'trav2'),
    ('102', 'Dressage du plateforme', 'm2', 3736.26, 'trav2'),
    ('103', 'Fouille d''ouvrage terrain ferme', 'm3', 9390.93, 'trav2'),
    ('104', 'Remblai d''ouvrage', 'm3', 37563.26, 'trav2'),
    ('105', 'Travaux d''implantation', 'fft', 152656.00, 'trav2');
insert into tache (num, designation, unite, pu, idTrav)
values
    ('201', 'Maçonnerie de moellons, ep= 35cm', 'm3', 172114.40, 'trav3'),
    ('202', 'Beton armée dosée à 350kg/m3 - semelles isolée', 'm3', 573215.80, 'trav3'),
    ('202', 'Beton armée dosée à 350kg/m3 - amorces poteaux', 'm3', 573215.80, 'trav3'),
    ('202', 'Beton armée dosée à 350kg/m3 - chaînage bas de 20x20', 'm3', 573215.80, 'trav3'),
    ('203', 'Remblai technique', 'm3', 37563.26, 'trav3'),
    ('204', 'Herrissonage ep=10', 'm3', 73245.40, 'trav3'),
    ('205', 'Beton ordinaire dosée à 300kg/m3 pour for', 'm3', 487815.80, 'trav3'),
    ('205', 'Chape de 2cm', 'm3', 33566.4, 'trav3');

create table typeMaison(
    idType AS ('type' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    designation varchar(100),
    durre int --en jour
);
insert into typeMaison (designation, durre)
values
    ('Maison en boit', 100),
    ('Maison en pierre', 120),
    ('Maison en parping', 140);

create table maison(
    idMaison AS ('maison' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    idType varchar(14) references typeMaison(idType) on delete cascade,
    nbrChambre int default 0,
    nbrToilet int default 0,
    nbrCuisine int default 0,
    nbrLiving int default 0
);
insert into maison (idType, nbrChambre, nbrToilet, nbrCuisine, nbrLiving)
values
    ('type1', 2, 1, 1, 1),
    ('type2', 3, 2, 2, 2),
    ('type3', 4, 3, 3, 3);
alter table maison add description varchar(100);
alter table maison add surface float default 0;
update maison set surface = 154 where idType = 'type1';
update maison set surface = 23.5 where idType = 'type2';
update maison set surface = 40.3 where idType = 'type3';
update maison set description = '2chambre, 1toilet, 1cuisine, 1living' where idType = 'type1';
update maison set description = '3chambre, 2toilet, 2cuisine, 2living' where idType = 'type2';
update maison set description = '4chambre, 3toilet, 3cuisine, 3living' where idType = 'type3';

-- create table devis(
--     idDevis AS ('devis' + cast(id as varchar(10))) PERSISTED primary key,
--     id int identity(1, 1),
--     designation varchar(50),
--     idTypeMaison varchar(14) references typeMaison(idType) on delete cascade,
-- );

CREATE TABLE devis (
    id int IDENTITY(1, 1) PRIMARY KEY,
    designation varchar(50),
    idTypeMaison varchar(14) REFERENCES typeMaison(idType) ON DELETE CASCADE,
    idDevis AS (
        CASE
            WHEN id < 100 THEN 'D' + RIGHT('000' + CAST(id AS varchar(10)), 2)
            ELSE 'D' + CAST(id AS varchar(10))
        END
    ) PERSISTED UNIQUE
);

insert into devis (designation, idTypeMaison)
values
    ('Devis maison boit', 'type1'),
    ('Devis maison pierre', 'type2'),
    ('Devis maison parping', 'type3');

create table detailDevis(
    idDetail AS ('detail' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    idDevis varchar(11) references devis(idDevis) on delete cascade,
    idTache varchar(15) references tache(idTache) on delete cascade,
    quantite float,
    pu int
);
ALTER TABLE detailDevis
ALTER COLUMN pu float;
insert into detailDevis (idDevis, idTache, quantite, pu)
values
    ('devis1', 'tache1', 26.98, (select pu from tache where idTache = 'tache1')),
    ('devis1', 'tache2', 101.36, (select pu from tache where idTache = 'tache2')),
    ('devis1', 'tache3', 101.36, (select pu from tache where idTache = 'tache3')),
    ('devis1', 'tache4', 24.44, (select pu from tache where idTache = 'tache4')),
    ('devis1', 'tache5', 15.59, (select pu from tache where idTache = 'tache5')),
    ('devis1', 'tache6', 1, (select pu from tache where idTache = 'tache6')),
    ('devis1', 'tache7', 9.62, (select pu from tache where idTache = 'tache7')),
    ('devis1', 'tache8', 0.53, (select pu from tache where idTache = 'tache8')),
    ('devis1', 'tache9', 0.56, (select pu from tache where idTache = 'tache9')),
    ('devis1', 'tache10', 2.44, (select pu from tache where idTache = 'tache10')),
    ('devis1', 'tache11', 15.59, (select pu from tache where idTache = 'tache11')),
    ('devis1', 'tache12', 7.90, (select pu from tache where idTache = 'tache12')),
    ('devis1', 'tache13', 9.62, (select pu from tache where idTache = 'tache13')),
    ('devis1', 'tache14', 9.22, (select pu from tache where idTache = 'tache14')),
    --
    ('devis2', 'tache1', 26.98, (select pu from tache where idTache = 'tache1')),
    ('devis2', 'tache2', 101.36, (select pu from tache where idTache = 'tache2')),
    ('devis2', 'tache3', 151.36, (select pu from tache where idTache = 'tache3')),
    ('devis2', 'tache4', 24.44, (select pu from tache where idTache = 'tache4')),
    ('devis2', 'tache5', 15.59, (select pu from tache where idTache = 'tache5')),
    ('devis2', 'tache6', 12, (select pu from tache where idTache = 'tache6')),
    ('devis2', 'tache7', 9.62, (select pu from tache where idTache = 'tache7')),
    ('devis2', 'tache8', 3.53, (select pu from tache where idTache = 'tache8')),
    ('devis2', 'tache9', 0.56, (select pu from tache where idTache = 'tache9')),
    --
    ('devis3', 'tache10', 12.44, (select pu from tache where idTache = 'tache10')),
    ('devis3', 'tache11', 115.59, (select pu from tache where idTache = 'tache11')),
    ('devis3', 'tache12', 7.90, (select pu from tache where idTache = 'tache12')),
    ('devis3', 'tache13', 49.62, (select pu from tache where idTache = 'tache13')),
    ('devis3', 'tache14', 29.22, (select pu from tache where idTache = 'tache14'));

--detail tache par devis -- type maison
select d.idDetail, d.idDevis, ty.idType, ty.designation, d.idTache, t.designation, d.quantite*t.pu as montantTache from detaildevis d
	join tache t on t.idTache = d.idTache
	join devis dev on dev.idDevis = d.idDevis
	join typeMaison ty on ty.idType = dev.idTypeMaison
	order by d.id

--ilaina amle le affichage volohany
create view v_devisTotalTypeMaison as
select d.idDevis, ty.idType, ty.designation, sum(d.quantite*t.pu) as montantTotal from detaildevis d
	join tache t on t.idTache = d.idTache
	join devis dev on dev.idDevis = d.idDevis
	join typeMaison ty on ty.idType = dev.idTypeMaison
	group by d.idDevis, ty.idType, ty.designation

create view v_info_maison as
SELECT m.idMaison, m.idType, ty.designation as type, m.nbrChambre, m.nbrToilet, m.nbrCuisine, m.nbrLiving, m.description, m.surface FROM maison m join typemaison ty on ty.idType = m.idType

    -- ito no tena mety
create view v_info_maison_total_devis as
select d.idDevis, d.montantTotal, m.idMaison, m.idType, m.type, m.nbrChambre, m.nbrToilet, m.nbrCuisine, m.nbrLiving, m.description, m.surface from v_devisTotalTypeMaison d
left join v_info_maison m  on m.idType = d.idType

create table finition(
    idFinition AS ('fini' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    designation varchar(50),
    pourcent float
);
insert into finition (designation, pourcent)
values
    ('Standard', 1),
    ('Gold', 3),
    ('Premium', 5),
    ('VIP', 15);

create table demandeDevis(
    idDemande AS ('demande' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    idUser varchar(11) references uuser(idUser),
    dateDebut date default getdate(),
    dateFin date default getdate(),
    idMaison varchar(16) references maison(idMaison) on delete cascade,
    idFinition varchar(14) references finition(idFinition) on delete cascade
);
alter table demandedevis add daty date default getDate();
alter table demandedevis add lieu varchar(100);

update demandedevis set daty = '2024-05-15' where idUser = 'u1';
update demandedevis set daty = '2023-05-15' where idUser = 'u2';

--view detail montant
create view v_detailDemandeDevis_montant as
select 
    d.idUser,
    d.idDemande,
	d.idMaison, 
	v.type, 
	d.idFinition, 
	f.designation, 
	f.pourcent, 
	d.dateDebut, 
	d.dateFin, 
	v.idDevis,
	CASE
		WHEN f.pourcent = 1 THEN v.montantTotal
		WHEN f.pourcent != 1 THEN (v.montantTotal * f.pourcent)/100 + v.montantTotal
	END AS montantTotal,
	d.daty
from demandeDevis d join finition f on f.idFinition = d.idFinition
left join v_info_maison_total_devis v on v.idMaison = d.idMaison

-- create table paiement(
--     idPaye AS ('paye' + cast(id as varchar(10))) PERSISTED primary key,
--     id int identity(1, 1),
--     idDemande varchar(17) references demandeDevis(idDemande) on delete cascade,
-- );

create table histo(
    idHisto AS ('histo' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    datePaye date default getdate(),
    paye int default 0
);
alter table histo add idDemande varchar(17) references demandeDevis(idDemande);
alter table histo add ref_paiement varchar(50);
-- insert into histo (paye, idDemande) values (0, 'demande6');

--reste 
create view v_detailDemandeDevis_montant_reste as
-- select v.idUser, v.idDemande, v.idMaison, v.type, v.idFinition, v.designation,v.pourcent, v.dateDebut, v.dateFin, v.idDevis,v.montantTotal, v.montantTotal - sum(h.paye) as reste from v_detailDemandeDevis_montant v 
-- 	left join histo h on h.idDemande = v.idDemande
-- 	group by v.idUser, v.idDemande, v.idMaison, v.type, v.idFinition, v.designation,v.pourcent, v.dateDebut, v.dateFin, v.idDevis,v.montantTotal
SELECT
    v.idUser,
    v.idDemande,
    v.idMaison,
    v.type,
    v.idFinition,
    v.designation,
    v.pourcent,
    v.dateDebut,
    v.dateFin,
    v.idDevis,
    v.montantTotal,
    COALESCE(v.montantTotal - SUM(h.paye), v.montantTotal) AS reste,
	v.daty
FROM
    v_detailDemandeDevis_montant v
LEFT JOIN
    histo h ON h.idDemande = v.idDemande
GROUP BY
    v.idUser,
    v.idDemande,
    v.idMaison,
    v.type,
    v.idFinition,
    v.designation,
    v.pourcent,
    v.dateDebut,
    v.dateFin,
    v.idDevis,
	v.daty,
    v.montantTotal;

--etat
create view v_detailDemandeDevis_montant_reste_etat as
select *,
    CASE
        WHEN reste != 0 AND reste != montantTotal THEN 'Avance'
        WHEN reste = 0 THEN 'Payer!'
        WHEN reste = montantTotal THEN 'NO payer'
    END AS EtatDePaiement
from v_detailDemandeDevis_montant_reste

--ilaina amn pdf
create view v_detail_devis as
select d.idDevis, t.travaux, t.designation, d.quantite, t.pu, d.quantite*t.pu as montant from detaildevis d
	join tache t on t.idTache = d.idTache
	join devis dev on dev.idDevis = d.idDevis

create table effectue (
    idEffectue AS ('eff' + cast(id as varchar(10))) PERSISTED primary key,
    id int identity(1, 1),
    idDemande varchar(17) references demandeDevis(idDemande),
    idMaison varchar(16) references maison(idMaison),
    type varchar(40),
    idFinition varchar(14) references finition(idFinition),
    designation varchar(40),
    dateDebut date default getdate(),
    dateFin date default getdate(),
    idDevis varchar(11) references devis(idDevis),
    montantTotal float
);
insert into effectue (idDemande, idMaison, type, idFinition, designation, dateDebut, dateFin, idDevis, montantTotal)
values (select idDemande, idMaison, type, idFinition, designation, dateDebut, dateFin, idDevis, montantTotal from v_detailDemandeDevis_montant_reste_etat where idDemande = 'demande1')

--chart
SELECT 
    DATEPART(MONTH, daty) AS Month,
    SUM(montantTotal) AS Montant 
FROM 
    v_detailDemandeDevis_montant_reste_etat 
WHERE 
    YEAR(daty) = @selectedyear
GROUP BY 
    DATEPART(MONTH, daty)
ORDER BY 
    Month;

--import
    --type maison
INSERT INTO typeMaison (designation, durre)
SELECT @designation, @durre
WHERE NOT EXISTS (
    SELECT 1
    FROM typeMaison
    WHERE designation = @designation
    AND durre = @durre
);

    --maison
INSERT INTO maison (idType, description, surface)
SELECT @idType, @description, @surface
WHERE NOT EXISTS (
    SELECT 1
    FROM maison
    WHERE idType = @idType
    AND description = @description
    AND surface = @surface
);


truncate table detaildevis
delete from devis
delete from histo
delete from demandeDevis
delete from maison
delete from typeMaison
delete from tache


truncate table detaildevis
truncate table devis
truncate table histo
truncate table demandeDevis
truncate table maison
truncate table typeMaison
truncate table tache


select * from detaildevis
select * from devis
select * from histo
select * from demandeDevis
select * from maison
select * from typeMaison
select * from tache



select * from devis
select * from detailDevis
select * from demandeDevis
select * from histo
select * from effectue
select * from travaux
select * from tache
select * from typeMaison
select * from maison


truncate table devis
truncate table demandeDevis
truncate table travaux
truncate table tache
truncate table typeMaison
truncate table maison



