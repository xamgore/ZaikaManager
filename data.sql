INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Kautzer-Abshire','New Dejonfurt','752-035-5366 x7886');
INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Rutherford-Legros','New Graciela','081-836-5568 x54105');
INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Hilll, Nader and Pouros','South Lilianefort','(327)025-1427 x6610');
INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Trantow, Weimann and Schaden','Port Dejaport','568.160.1331 x4418');
INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Jacobson-Cartwright','Alvertaberg','286.257.2558');
INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Watsica-Kreiger','West Elfriedaview','403-355-4783 x371');
INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Franecki LLC','Port Coralie','(117)671-0567 x20524');
INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Wiza Group','East Mozelle','(782)672-6073 x05232');
INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Mante-Jast','Port Anika','122.760.3340');
INSERT INTO "Producers" ("Name","City","Phone") VALUES ('Ferry LLC','Kuphalton','1-785-870-0818');


INSERT INTO "Products" ("Name","Measure") VALUES ('bunny','thing');
INSERT INTO "Products" ("Name","Measure") VALUES ('puppy','thing');
INSERT INTO "Products" ("Name","Measure") VALUES ('kitten','thing');
INSERT INTO "Products" ("Name","Measure") VALUES ('octopus','thing');
INSERT INTO "Products" ("Name","Measure") VALUES ('piggy','thing');
INSERT INTO "Products" ("Name","Measure") VALUES ('fox','thing');


INSERT INTO "Warehouses" ("Name","City") VALUES ('Huels Via','Raheemfort');
INSERT INTO "Warehouses" ("Name","City") VALUES ('Myrtle Ways','Port Justen');
INSERT INTO "Warehouses" ("Name","City") VALUES ('Lorenz Mill','Bartellview');



INSERT INTO "Operations" VALUES (DEFAULT, 1, 1, 1, 400, 300, '2014.12.01');
INSERT INTO "Operations" VALUES (DEFAULT, 1, 1, 1, 600, -100, '2014.12.01');
INSERT INTO "Operations" VALUES (DEFAULT, 2, 1, 1, 100, 570, '2014.12.01');
INSERT INTO "Operations" VALUES (DEFAULT, 3, 2, 2, 99, 99, '2013.04.03');

REFRESH MATERIALIZED VIEW "LastOperations";
