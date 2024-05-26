USE [BankDB]
GO
SET IDENTITY_INSERT [dbo].[emp_info] ON 

INSERT [dbo].[emp_info] ([emp_id], [emp_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [hire_date], [termination_date]) VALUES (1, N'A00000001', N'Edward', NULL, N'Dudley', NULL, CAST(N'1964-06-03' AS Date), CAST(N'1980-05-30' AS Date), NULL)
INSERT [dbo].[emp_info] ([emp_id], [emp_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [hire_date], [termination_date]) VALUES (2, N'A00000002', N'Crystal', NULL, N'Ford', NULL, CAST(N'1905-07-14' AS Date), CAST(N'1984-06-24' AS Date), NULL)
INSERT [dbo].[emp_info] ([emp_id], [emp_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [hire_date], [termination_date]) VALUES (3, N'A00000003', N'Marcus', N'Kim', N'Martin', NULL, CAST(N'1951-07-18' AS Date), CAST(N'1987-07-09' AS Date), NULL)
INSERT [dbo].[emp_info] ([emp_id], [emp_secondary_id], [first_name], [middle_name], [last_name], [suffix], [date_of_birth], [hire_date], [termination_date]) VALUES (4, N'A00000004', N'Peter', N'Mitchell', N'Fisher', NULL, CAST(N'1980-10-23' AS Date), CAST(N'2022-10-13' AS Date), NULL)
SET IDENTITY_INSERT [dbo].[emp_info] OFF
GO
SET IDENTITY_INSERT [dbo].[emp_contact] ON 

INSERT [dbo].[emp_contact] ([emp_id], [emp_email], [emp_phone], [emp_address], [emp_address_2], [emp_city_id]) VALUES (1, N'black_rest@example.com', N'374-846-9994', N'592 Legion Rd', NULL, 12713)
INSERT [dbo].[emp_contact] ([emp_id], [emp_email], [emp_phone], [emp_address], [emp_address_2], [emp_city_id]) VALUES (2, N'yellowf@example.com', N'213-462-6417', N'2098 Coronado Ave', NULL, 2696)
INSERT [dbo].[emp_contact] ([emp_id], [emp_email], [emp_phone], [emp_address], [emp_address_2], [emp_city_id]) VALUES (3, N'friend.forge@example.com', N'418-285-9240', N'4080 Beverly Path', NULL, 35091)
INSERT [dbo].[emp_contact] ([emp_id], [emp_email], [emp_phone], [emp_address], [emp_address_2], [emp_city_id]) VALUES (4, N'fisher.box@example.com', N'285-614-6398', N'782 Sherman Trl', N'Apt 832', 15626)
SET IDENTITY_INSERT [dbo].[emp_contact] OFF
GO
INSERT [dbo].[emp_rep_id] ([emp_id], [rep_id]) VALUES (4, N'0MTJN')
INSERT [dbo].[emp_rep_id] ([emp_id], [rep_id]) VALUES (3, N'STU56')
INSERT [dbo].[emp_rep_id] ([emp_id], [rep_id]) VALUES (1, N'XCF96')
INSERT [dbo].[emp_rep_id] ([emp_id], [rep_id]) VALUES (2, N'ZZWCP')
GO
SET IDENTITY_INSERT [dbo].[emp_position] ON 

INSERT [dbo].[emp_position] ([emp_position_id], [emp_id], [position_location_id], [start_date], [end_date]) VALUES (1, 1, 444, CAST(N'1980-05-30' AS Date), NULL)
INSERT [dbo].[emp_position] ([emp_position_id], [emp_id], [position_location_id], [start_date], [end_date]) VALUES (2, 2, 3702, CAST(N'1984-06-24' AS Date), NULL)
INSERT [dbo].[emp_position] ([emp_position_id], [emp_id], [position_location_id], [start_date], [end_date]) VALUES (3, 3, 2239, CAST(N'1987-07-09' AS Date), NULL)
INSERT [dbo].[emp_position] ([emp_position_id], [emp_id], [position_location_id], [start_date], [end_date]) VALUES (4, 4, 2034, CAST(N'2022-10-13' AS Date), NULL)
SET IDENTITY_INSERT [dbo].[emp_position] OFF
GO
INSERT [dbo].[emp_pass] ([emp_id], [decrypted_pass], [emp_pass_hash], [emp_pass_salt]) VALUES (1, N'_9Faf2wu', NULL, NULL)
INSERT [dbo].[emp_pass] ([emp_id], [decrypted_pass], [emp_pass_hash], [emp_pass_salt]) VALUES (2, N'V2Vk)2Dd', NULL, NULL)
INSERT [dbo].[emp_pass] ([emp_id], [decrypted_pass], [emp_pass_hash], [emp_pass_salt]) VALUES (3, N')&05Irkd', NULL, NULL)
INSERT [dbo].[emp_pass] ([emp_id], [decrypted_pass], [emp_pass_hash], [emp_pass_salt]) VALUES (4, N'$1sbcWwp', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[emp_tax] ON 

INSERT [dbo].[emp_tax] ([emp_id], [decrypted_tax], [encrypted_emp_tax_id], [emp_tax_id_hash]) VALUES (1, N'335-93-2044', NULL, NULL)
INSERT [dbo].[emp_tax] ([emp_id], [decrypted_tax], [encrypted_emp_tax_id], [emp_tax_id_hash]) VALUES (2, N'426-63-3039', NULL, NULL)
INSERT [dbo].[emp_tax] ([emp_id], [decrypted_tax], [encrypted_emp_tax_id], [emp_tax_id_hash]) VALUES (3, N'345-56-7344', NULL, NULL)
INSERT [dbo].[emp_tax] ([emp_id], [decrypted_tax], [encrypted_emp_tax_id], [emp_tax_id_hash]) VALUES (4, N'223-27-6312', NULL, NULL)
SET IDENTITY_INSERT [dbo].[emp_tax] OFF
GO
SET IDENTITY_INSERT [dbo].[emp_salary] ON 

INSERT [dbo].[emp_salary] ([emp_salary_id], [emp_id], [effective_date], [end_date], [salary_amount]) VALUES (1, 1, CAST(N'1980-05-30' AS Date), NULL, CAST(90635.00 AS Decimal(10, 2)))
INSERT [dbo].[emp_salary] ([emp_salary_id], [emp_id], [effective_date], [end_date], [salary_amount]) VALUES (2, 2, CAST(N'1984-06-24' AS Date), NULL, CAST(31104.00 AS Decimal(10, 2)))
INSERT [dbo].[emp_salary] ([emp_salary_id], [emp_id], [effective_date], [end_date], [salary_amount]) VALUES (3, 3, CAST(N'1987-07-09' AS Date), NULL, CAST(166886.00 AS Decimal(10, 2)))
INSERT [dbo].[emp_salary] ([emp_salary_id], [emp_id], [effective_date], [end_date], [salary_amount]) VALUES (4, 4, CAST(N'2022-10-13' AS Date), NULL, CAST(101542.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[emp_salary] OFF
GO

USE BankDB;
OPEN SYMMETRIC KEY TaxEncryption
DECRYPTION BY CERTIFICATE TaxCertificate;

UPDATE [dbo].[emp_tax]
SET [emp_tax_id_hash] = HASHBYTES('SHA2_256', [decrypted_tax]),
	[encrypted_emp_tax_id] = EncryptByKey(Key_GUID('TaxEncryption'), [decrypted_tax]);

CLOSE SYMMETRIC KEY TaxEncryption;
GO

USE BankDB;
ALTER TABLE [dbo].[emp_tax]
ALTER COLUMN [emp_tax_id_hash] varbinary(64) NOT NULL;
GO

USE BankDB;
ALTER TABLE [dbo].[emp_tax]
ALTER COLUMN [encrypted_emp_tax_id] varbinary(MAX) NOT NULL;
GO

USE BankDB;
ALTER TABLE [dbo].[emp_tax]
DROP COLUMN [decrypted_tax];
GO

USE BankDB;
GO

IF OBJECT_ID('tempdb..#TempPasswords') IS NOT NULL
    DROP TABLE #TempPasswords;

CREATE TABLE #TempPasswords (
    emp_id BIGINT,
    emp_pass_salt UNIQUEIDENTIFIER,
    emp_pass_hash VARBINARY(32)
);

INSERT INTO #TempPasswords (emp_id, emp_pass_salt)
SELECT 
    emp_id, 
    NEWID()
FROM dbo.emp_pass;

UPDATE t
SET t.emp_pass_hash = HASHBYTES('SHA2_256', CONCAT(p.decrypted_pass, CAST(t.emp_pass_salt AS NVARCHAR(36))))
FROM #TempPasswords t
JOIN dbo.emp_pass p ON t.emp_id = p.emp_id;

UPDATE p
SET 
    p.emp_pass_salt = t.emp_pass_salt,
    p.emp_pass_hash = t.emp_pass_hash
FROM dbo.emp_pass p
JOIN #TempPasswords t ON p.emp_id = t.emp_id;

DROP TABLE #TempPasswords;
GO

USE BankDB;
ALTER TABLE [dbo].[emp_pass]
DROP COLUMN [decrypted_pass];
GO