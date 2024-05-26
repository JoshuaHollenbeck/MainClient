USE [BankDB]
GO
SET IDENTITY_INSERT [dbo].[cust_info] ON 

INSERT [dbo].[cust_info] ([cust_id], [cust_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [client_since], [emp_id]) VALUES (1, N'050670903657', N'John', N'Christopher', N'Mcgrath', NULL, CAST(N'1962-12-06' AS Date), CAST(N'1970-02-12' AS Date), NULL)
INSERT [dbo].[cust_info] ([cust_id], [cust_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [client_since], [emp_id]) VALUES (2, N'075926855051', N'Jack', N'Johnny', N'Mcgrath', NULL, CAST(N'1920-02-25' AS Date), CAST(N'1970-02-12' AS Date), NULL)
INSERT [dbo].[cust_info] ([cust_id], [cust_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [client_since], [emp_id]) VALUES (3, N'026182590391', N'Melvin', NULL, N'Mcgee', NULL, CAST(N'1892-03-25' AS Date), CAST(N'1970-03-07' AS Date), NULL)
INSERT [dbo].[cust_info] ([cust_id], [cust_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [client_since], [emp_id]) VALUES (4, N'054024729144', N'Samantha', NULL, N'Cabrera', NULL, CAST(N'1920-07-08' AS Date), CAST(N'1970-06-26' AS Date), NULL)
INSERT [dbo].[cust_info] ([cust_id], [cust_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [client_since], [emp_id]) VALUES (5, N'023133453189', N'Lisa', NULL, N'Moreno', NULL, CAST(N'1931-08-02' AS Date), CAST(N'1970-07-23' AS Date), NULL)
INSERT [dbo].[cust_info] ([cust_id], [cust_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [client_since], [emp_id]) VALUES (6, N'003033323457', N'Erin', NULL, N'Cummings', NULL, CAST(N'1948-03-31' AS Date), CAST(N'1970-08-11' AS Date), NULL)
INSERT [dbo].[cust_info] ([cust_id], [cust_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [client_since], [emp_id]) VALUES (7, N'046186366253', N'Richard', NULL, N'Cummings', NULL, CAST(N'1898-08-28' AS Date), CAST(N'1970-08-11' AS Date), NULL)
INSERT [dbo].[cust_info] ([cust_id], [cust_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [client_since], [emp_id]) VALUES (8, N'012178561556', N'Yolanda', NULL, N'Armstrong', NULL, CAST(N'1908-08-26' AS Date), CAST(N'1970-08-11' AS Date), NULL)
SET IDENTITY_INSERT [dbo].[cust_info] OFF
GO

USE [BankDB]
GO
INSERT [dbo].[cust_contact] ([cust_id], [cust_email], [cust_phone_home], [cust_phone_business], [cust_address], [cust_address_2], [cust_city_id]) VALUES (1, N'mcgrath_corner@example.com', N'860-108-4394', NULL, N'3332 Belvedere Hl', NULL, 772)
INSERT [dbo].[cust_contact] ([cust_id], [cust_email], [cust_phone_home], [cust_phone_business], [cust_address], [cust_address_2], [cust_city_id]) VALUES (2, N'mcgrathj@example.com', N'738-433-3445', NULL, N'3332 Belvedere Hl', N'Apt 55', 772)
INSERT [dbo].[cust_contact] ([cust_id], [cust_email], [cust_phone_home], [cust_phone_business], [cust_address], [cust_address_2], [cust_city_id]) VALUES (3, N'knolls.melvin@example.com', N'949-242-1821', N'716-552-5148', N'451 Fairbanks Plz', N'Unit 633', 29378)
INSERT [dbo].[cust_contact] ([cust_id], [cust_email], [cust_phone_home], [cust_phone_business], [cust_address], [cust_address_2], [cust_city_id]) VALUES (4, N'cabrera04028@example.com', N'238-364-8066', NULL, N'4884 Farnum St', NULL, 10879)
INSERT [dbo].[cust_contact] ([cust_id], [cust_email], [cust_phone_home], [cust_phone_business], [cust_address], [cust_address_2], [cust_city_id]) VALUES (5, N'lisa_should@example.com', N'448-491-4067', N'581-398-4198', N'1002 Salmon Walk', NULL, 37360)
INSERT [dbo].[cust_contact] ([cust_id], [cust_email], [cust_phone_home], [cust_phone_business], [cust_address], [cust_address_2], [cust_city_id]) VALUES (6, N'c_erin@example.com', N'272-862-3993', NULL, N'5481 Stanford Heights Pass', NULL, 27809)
INSERT [dbo].[cust_contact] ([cust_id], [cust_email], [cust_phone_home], [cust_phone_business], [cust_address], [cust_address_2], [cust_city_id]) VALUES (7, N'cr@example.com', N'522-961-1663', NULL, N'5481 Stanford Heights Pass', NULL, 27809)
INSERT [dbo].[cust_contact] ([cust_id], [cust_email], [cust_phone_home], [cust_phone_business], [cust_address], [cust_address_2], [cust_city_id]) VALUES (8, N'ideablue@example.com', N'975-399-6322', NULL, N'5724 San Diego Path', NULL, 6203)
GO

USE [BankDB]
GO
INSERT [dbo].[cust_emp] ([cust_id], [employment_status], [employer_name], [occupation]) VALUES (1, 1, N'Nor LLC', N'Pilot')
INSERT [dbo].[cust_emp] ([cust_id], [employment_status], [employer_name], [occupation]) VALUES (2, 0, NULL, NULL)
INSERT [dbo].[cust_emp] ([cust_id], [employment_status], [employer_name], [occupation]) VALUES (3, 1, N'Brown & Sons', N'Trade Union Research Officer')
INSERT [dbo].[cust_emp] ([cust_id], [employment_status], [employer_name], [occupation]) VALUES (4, 1, N'Single Inc', N'Catering Manager')
INSERT [dbo].[cust_emp] ([cust_id], [employment_status], [employer_name], [occupation]) VALUES (5, 1, N'Alexander & Sons', N'Field Trials Officer')
INSERT [dbo].[cust_emp] ([cust_id], [employment_status], [employer_name], [occupation]) VALUES (6, 1, N'White Group', N'Retail Buyer')
INSERT [dbo].[cust_emp] ([cust_id], [employment_status], [employer_name], [occupation]) VALUES (7, 1, N'Aqua Ago PLC', N'Historic Buildings Inspector')
INSERT [dbo].[cust_emp] ([cust_id], [employment_status], [employer_name], [occupation]) VALUES (8, 1, N'Movement Fuchsia PLC', N'Politician''S Assistant')
GO

USE [BankDB]
GO
INSERT [dbo].[cust_id] ([cust_id], [id_type], [id_state], [id_num], [id_exp], [mothers_maiden]) VALUES (1, 1, 22, N'639583458', N'9/2034', N'Williams')
INSERT [dbo].[cust_id] ([cust_id], [id_type], [id_state], [id_num], [id_exp], [mothers_maiden]) VALUES (2, 5, 22, N'8547553', N'11/2030', N'Garcia')
INSERT [dbo].[cust_id] ([cust_id], [id_type], [id_state], [id_num], [id_exp], [mothers_maiden]) VALUES (3, 4, 4, N'W72821116', N'10/2032', N'Olson')
INSERT [dbo].[cust_id] ([cust_id], [id_type], [id_state], [id_num], [id_exp], [mothers_maiden]) VALUES (4, 5, 41, N'007899724', N'11/2024', N'Peterson')
INSERT [dbo].[cust_id] ([cust_id], [id_type], [id_state], [id_num], [id_exp], [mothers_maiden]) VALUES (5, 4, 5, N'B99180781', N'2/2027', N'Gardner')
INSERT [dbo].[cust_id] ([cust_id], [id_type], [id_state], [id_num], [id_exp], [mothers_maiden]) VALUES (6, 1, 28, N'W61096310', N'5/2032', N'Beltran')
INSERT [dbo].[cust_id] ([cust_id], [id_type], [id_state], [id_num], [id_exp], [mothers_maiden]) VALUES (7, 1, 28, N'886814332', N'6/2032', N'Ross')
INSERT [dbo].[cust_id] ([cust_id], [id_type], [id_state], [id_num], [id_exp], [mothers_maiden]) VALUES (8, 2, 39, N'Y25307916', N'3/2027', N'Jones')
GO

USE [BankDB]
GO
INSERT [dbo].[cust_privacy] ([cust_id], [voice_auth], [do_not_call], [share_affiliates]) VALUES (1, 0, 0, 0)
INSERT [dbo].[cust_privacy] ([cust_id], [voice_auth], [do_not_call], [share_affiliates]) VALUES (2, 1, 1, 0)
INSERT [dbo].[cust_privacy] ([cust_id], [voice_auth], [do_not_call], [share_affiliates]) VALUES (3, 1, 0, 0)
INSERT [dbo].[cust_privacy] ([cust_id], [voice_auth], [do_not_call], [share_affiliates]) VALUES (4, 1, 1, 1)
INSERT [dbo].[cust_privacy] ([cust_id], [voice_auth], [do_not_call], [share_affiliates]) VALUES (5, 1, 0, 0)
INSERT [dbo].[cust_privacy] ([cust_id], [voice_auth], [do_not_call], [share_affiliates]) VALUES (6, 0, 1, 1)
INSERT [dbo].[cust_privacy] ([cust_id], [voice_auth], [do_not_call], [share_affiliates]) VALUES (7, 0, 1, 1)
INSERT [dbo].[cust_privacy] ([cust_id], [voice_auth], [do_not_call], [share_affiliates]) VALUES (8, 0, 0, 0)
GO

USE [BankDB]
GO
INSERT [dbo].[cust_tax] ([cust_id], [decrypted_tax], [encrypted_cust_tax_id], [cust_tax_id_hash]) VALUES (1, N'810-27-9984', NULL, NULL)
INSERT [dbo].[cust_tax] ([cust_id], [decrypted_tax], [encrypted_cust_tax_id], [cust_tax_id_hash]) VALUES (2, N'128-85-7954', NULL, NULL)
INSERT [dbo].[cust_tax] ([cust_id], [decrypted_tax], [encrypted_cust_tax_id], [cust_tax_id_hash]) VALUES (3, N'823-76-1757', NULL, NULL)
INSERT [dbo].[cust_tax] ([cust_id], [decrypted_tax], [encrypted_cust_tax_id], [cust_tax_id_hash]) VALUES (4, N'305-36-6961', NULL, NULL)
INSERT [dbo].[cust_tax] ([cust_id], [decrypted_tax], [encrypted_cust_tax_id], [cust_tax_id_hash]) VALUES (5, N'563-37-3512', NULL, NULL)
INSERT [dbo].[cust_tax] ([cust_id], [decrypted_tax], [encrypted_cust_tax_id], [cust_tax_id_hash]) VALUES (6, N'794-86-3728', NULL, NULL)
INSERT [dbo].[cust_tax] ([cust_id], [decrypted_tax], [encrypted_cust_tax_id], [cust_tax_id_hash]) VALUES (7, N'984-35-6422', NULL, NULL)
INSERT [dbo].[cust_tax] ([cust_id], [decrypted_tax], [encrypted_cust_tax_id], [cust_tax_id_hash]) VALUES (8, N'257-43-6077', NULL, NULL)
GO

USE BankDB;
OPEN SYMMETRIC KEY TaxEncryption
DECRYPTION BY CERTIFICATE TaxCertificate;

UPDATE [dbo].[cust_tax]
SET [cust_tax_id_hash] = HASHBYTES('SHA2_256', [decrypted_tax]),
	[encrypted_cust_tax_id] = EncryptByKey(Key_GUID('TaxEncryption'), [decrypted_tax]);

CLOSE SYMMETRIC KEY TaxEncryption;
GO

USE BankDB;
ALTER TABLE [dbo].[cust_tax]
ALTER COLUMN [encrypted_cust_tax_id] varbinary(MAX) NOT NULL;
GO

USE BankDB;
ALTER TABLE [dbo].[cust_tax]
ALTER COLUMN [cust_tax_id_hash] varbinary(64) NOT NULL;
GO

USE BankDB;
ALTER TABLE [dbo].[cust_tax]
DROP COLUMN [decrypted_tax];
GO