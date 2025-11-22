-- Patient Data Generation Script
-- This script generates sample patient data for the HCAS database
-- Execute this script in SQL Server Management Studio or via sqlcmd

SET IDENTITY_INSERT [dbo].[Patients] ON
GO

-- Insert sample patient records
INSERT [dbo].[Patients] ([Id], [Name], [DateOfBirth], [Gender], [Phone], [Email], [del_flg]) VALUES 
(2, N'John Anderson', CAST(N'1985-03-22' AS Date), N'Male', N'555-0101', N'john.anderson@email.com', 0),
(3, N'Maria Garcia', CAST(N'1992-07-08' AS Date), N'Female', N'555-0102', N'maria.garcia@email.com', 0),
(4, N'Robert Chen', CAST(N'1978-11-15' AS Date), N'Male', N'555-0103', N'robert.chen@email.com', 0),
(5, N'Emily Johnson', CAST(N'1995-02-28' AS Date), N'Female', N'555-0104', N'emily.johnson@email.com', 0),
(6, N'Michael Brown', CAST(N'1989-09-10' AS Date), N'Male', N'555-0105', N'michael.brown@email.com', 0),
(7, N'Sarah Wilson', CAST(N'1991-06-14' AS Date), N'Female', N'555-0106', N'sarah.wilson@email.com', 0),
(8, N'David Martinez', CAST(N'1982-12-05' AS Date), N'Male', N'555-0107', N'david.martinez@email.com', 0),
(9, N'Jennifer Lee', CAST(N'1994-04-18' AS Date), N'Female', N'555-0108', N'jennifer.lee@email.com', 0),
(10, N'James Taylor', CAST(N'1987-08-25' AS Date), N'Male', N'555-0109', N'james.taylor@email.com', 0),
(11, N'Lisa Anderson', CAST(N'1993-01-30' AS Date), N'Female', N'555-0110', N'lisa.anderson@email.com', 0),
(12, N'Christopher White', CAST(N'1980-05-12' AS Date), N'Male', N'555-0111', N'christopher.white@email.com', 0),
(13, N'Amanda Harris', CAST(N'1996-10-07' AS Date), N'Female', N'555-0112', N'amanda.harris@email.com', 0),
(14, N'Daniel Thomas', CAST(N'1984-03-19' AS Date), N'Male', N'555-0113', N'daniel.thomas@email.com', 0),
(15, N'Jessica Moore', CAST(N'1990-07-23' AS Date), N'Female', N'555-0114', N'jessica.moore@email.com', 0),
(16, N'Matthew Jackson', CAST(N'1986-11-02' AS Date), N'Male', N'555-0115', N'matthew.jackson@email.com', 0),
(17, N'Ashley Thompson', CAST(N'1992-02-14' AS Date), N'Female', N'555-0116', N'ashley.thompson@email.com', 0),
(18, N'Andrew Davis', CAST(N'1979-08-09' AS Date), N'Male', N'555-0117', N'andrew.davis@email.com', 0),
(19, N'Michelle Miller', CAST(N'1994-12-21' AS Date), N'Female', N'555-0118', N'michelle.miller@email.com', 0),
(20, N'Ryan Garcia', CAST(N'1988-04-04' AS Date), N'Male', N'555-0119', N'ryan.garcia@email.com', 0),
(21, N'Nicole Rodriguez', CAST(N'1991-09-16' AS Date), N'Female', N'555-0120', N'nicole.rodriguez@email.com', 0),
(22, N'Kevin Lewis', CAST(N'1983-01-27' AS Date), N'Male', N'555-0121', N'kevin.lewis@email.com', 0),
(23, N'Stephanie Walker', CAST(N'1995-06-08' AS Date), N'Female', N'555-0122', N'stephanie.walker@email.com', 0),
(24, N'Brandon Hall', CAST(N'1987-10-31' AS Date), N'Male', N'555-0123', N'brandon.hall@email.com', 0),
(25, N'Rachel Allen', CAST(N'1993-03-13' AS Date), N'Female', N'555-0124', N'rachel.allen@email.com', 0),
(26, N'Jonathan Young', CAST(N'1981-07-24' AS Date), N'Male', N'555-0125', N'jonathan.young@email.com', 0),
(27, N'Samantha King', CAST(N'1996-11-05' AS Date), N'Female', N'555-0126', N'samantha.king@email.com', 0),
(28, N'Justin Wright', CAST(N'1989-02-17' AS Date), N'Male', N'555-0127', N'justin.wright@email.com', 0),
(29, N'Lauren Scott', CAST(N'1994-08-29' AS Date), N'Female', N'555-0128', N'lauren.scott@email.com', 0),
(30, N'Benjamin Torres', CAST(N'1985-12-11' AS Date), N'Male', N'555-0129', N'benjamin.torres@email.com', 0),
(31, N'Megan Green', CAST(N'1990-05-22' AS Date), N'Female', N'555-0130', N'megan.green@email.com', 0),
(32, N'Nathan Adams', CAST(N'1982-09-03' AS Date), N'Male', N'555-0131', N'nathan.adams@email.com', 0),
(33, N'Brittany Nelson', CAST(N'1997-01-15' AS Date), N'Female', N'555-0132', N'brittany.nelson@email.com', 0),
(34, N'Jordan Baker', CAST(N'1988-04-26' AS Date), N'Male', N'555-0133', N'jordan.baker@email.com', 0),
(35, N'Katherine Hill', CAST(N'1992-08-07' AS Date), N'Female', N'555-0134', N'katherine.hill@email.com', 0),
(36, N'Tyler Ramirez', CAST(N'1984-11-18' AS Date), N'Male', N'555-0135', N'tyler.ramirez@email.com', 0),
(37, N'Christina Campbell', CAST(N'1995-03-01' AS Date), N'Female', N'555-0136', N'christina.campbell@email.com', 0),
(38, N'Zachary Mitchell', CAST(N'1986-07-13' AS Date), N'Male', N'555-0137', N'zachary.mitchell@email.com', 0),
(39, N'Kayla Roberts', CAST(N'1991-10-24' AS Date), N'Female', N'555-0138', N'kayla.roberts@email.com', 0),
(40, N'Austin Carter', CAST(N'1983-02-06' AS Date), N'Male', N'555-0139', N'austin.carter@email.com', 0),
(41, N'Hannah Phillips', CAST(N'1996-06-17' AS Date), N'Female', N'555-0140', N'hannah.phillips@email.com', 0),
(42, N'Jacob Evans', CAST(N'1989-09-28' AS Date), N'Male', N'555-0141', N'jacob.evans@email.com', 0),
(43, N'Olivia Turner', CAST(N'1994-01-09' AS Date), N'Female', N'555-0142', N'olivia.turner@email.com', 0),
(44, N'Ethan Parker', CAST(N'1987-05-20' AS Date), N'Male', N'555-0143', N'ethan.parker@email.com', 0),
(45, N'Madison Collins', CAST(N'1993-08-31' AS Date), N'Female', N'555-0144', N'madison.collins@email.com', 0),
(46, N'Noah Stewart', CAST(N'1981-12-12' AS Date), N'Male', N'555-0145', N'noah.stewart@email.com', 0),
(47, N'Chloe Morris', CAST(N'1995-04-23' AS Date), N'Female', N'555-0146', N'chloe.morris@email.com', 0),
(48, N'Logan Rogers', CAST(N'1988-08-04' AS Date), N'Male', N'555-0147', N'logan.rogers@email.com', 0),
(49, N'Sophia Reed', CAST(N'1992-11-15' AS Date), N'Female', N'555-0148', N'sophia.reed@email.com', 0),
(50, N'Mason Cook', CAST(N'1984-03-27' AS Date), N'Male', N'555-0149', N'mason.cook@email.com', 0);

SET IDENTITY_INSERT [dbo].[Patients] OFF
GO

-- Verify inserted records
SELECT COUNT(*) AS TotalPatients FROM [dbo].[Patients] WHERE [del_flg] = 0;
GO
