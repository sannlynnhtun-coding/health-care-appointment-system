-- Doctor Data Generation Script
-- This script generates sample doctor data for the HCAS database
-- Execute this script in SQL Server Management Studio or via sqlcmd
-- Note: This script assumes Specializations (1-20) already exist
-- SpecializationId 1: Cardiologist (နှလုံးအထူးကု)
-- SpecializationId 2: General Practitioner (အ​ထွေ​ထွေရောဂါကု)
-- SpecializationId 3: Gastroenterology (အစာအိမ်နှင့်အူလမ်း​ကြောင်းအထူးကု)
-- SpecializationId 4-20: Various other medical specializations

SET IDENTITY_INSERT [dbo].[Doctors] ON
GO

-- Insert sample doctor records
-- Note: IDs 1, 2, 4, 5, 6 already exist - starting from ID 3 and continuing from 7
-- Doctors are distributed across all 20 specializations
INSERT [dbo].[Doctors] ([Id], [Name], [SpecializationId], [del_flg]) VALUES 
(3, N'Dr. Aung Kyaw', 1, 0),
(7, N'Dr. Mya Mya', 2, 0),
(8, N'Dr. Zaw Win', 3, 0),
(9, N'Dr. Khin Hnin', 4, 0),
(10, N'Dr. Soe Thiha', 5, 0),
(11, N'Dr. Win Myint', 6, 0),
(12, N'Dr. Thazin Aung', 7, 0),
(13, N'Dr. Min Zaw', 8, 0),
(14, N'Dr. Nilar Kyaw', 9, 0),
(15, N'Dr. Hla Hla', 10, 0),
(16, N'Dr. Kyaw Thura', 11, 0),
(17, N'Dr. Su Su', 12, 0),
(18, N'Dr. Aung Min', 13, 0),
(19, N'Dr. May Myat', 14, 0),
(20, N'Dr. Than Zaw', 15, 0),
(21, N'Dr. Nyein Chan', 16, 0),
(22, N'Dr. Phyo Thazin', 17, 0),
(23, N'Dr. Zin Mar', 18, 0),
(24, N'Dr. Myo Aung', 19, 0),
(25, N'Dr. Ei Ei', 20, 0),
(26, N'Dr. Htet Aung', 1, 0),
(27, N'Dr. Khin Su', 2, 0),
(28, N'Dr. Thant Zin', 3, 0),
(29, N'Dr. Wai Yan', 4, 0),
(30, N'Dr. Sandar', 5, 0),
(31, N'Dr. Myint Swe', 6, 0),
(32, N'Dr. Hla Myo', 7, 0),
(33, N'Dr. Aye Aye', 8, 0),
(34, N'Dr. Tun Tun', 9, 0),
(35, N'Dr. Moe Moe', 10, 0),
(36, N'Dr. Aung Ko', 11, 0),
(37, N'Dr. Myint Myint', 12, 0),
(38, N'Dr. Zaw Oo', 13, 0),
(39, N'Dr. Nilar', 14, 0),
(40, N'Dr. Win Ko', 15, 0),
(41, N'Dr. Su Myat', 16, 0),
(42, N'Dr. Thein Zaw', 17, 0),
(43, N'Dr. Thanda', 18, 0),
(44, N'Dr. Zaw Min', 19, 0),
(45, N'Dr. Mya Thida', 20, 0),
(46, N'Dr. Kyaw Kyaw', 1, 0),
(47, N'Dr. San San', 2, 0),
(48, N'Dr. Aung Hlaing', 3, 0),
(49, N'Dr. May Thwe', 4, 0),
(50, N'Dr. Zaw Htet', 5, 0),
(51, N'Dr. Hlaing Hlaing', 6, 0),
(52, N'Dr. Myint San', 7, 0),
(53, N'Dr. Nyein Nyein', 8, 0),
(54, N'Dr. Ko Ko', 9, 0),
(55, N'Dr. Phyu Phyu', 10, 0),
(56, N'Dr. Min Min', 11, 0),
(57, N'Dr. Aung Phyo', 12, 0),
(58, N'Dr. Thandar', 13, 0),
(59, N'Dr. Zaw Zaw', 14, 0),
(60, N'Dr. Hnin Hnin', 15, 0),
(61, N'Dr. Thura Kyaw', 16, 0),
(62, N'Dr. Yadanar', 17, 0),
(63, N'Dr. Soe Win', 18, 0),
(64, N'Dr. Phyo Min', 19, 0),
(65, N'Dr. Thet Wai', 20, 0),
(66, N'Dr. Zaw Myo', 1, 0),
(67, N'Dr. Hnin Yee', 2, 0),
(68, N'Dr. Kyaw Soe', 3, 0),
(69, N'Dr. Myat Mon', 4, 0),
(70, N'Dr. Win Htet', 5, 0),
(71, N'Dr. Su Mon', 6, 0),
(72, N'Dr. Zin Win', 7, 0),
(73, N'Dr. Thida Win', 8, 0),
(74, N'Dr. Myint Thein', 9, 0),
(75, N'Dr. Aung Thein', 10, 0),
(76, N'Dr. Yamin Kyaw', 11, 0),
(77, N'Dr. Phyo Wai', 12, 0),
(78, N'Dr. Thant Win', 13, 0),
(79, N'Dr. Htet Win', 14, 0),
(80, N'Dr. Zaw Phyo', 15, 0);

SET IDENTITY_INSERT [dbo].[Doctors] OFF
GO

-- Verify inserted records by specialization
SELECT 
    s.[Id] AS SpecializationId,
    s.[Name] AS Specialization,
    COUNT(*) AS TotalDoctors,
    SUM(CASE WHEN d.[del_flg] = 0 THEN 1 ELSE 0 END) AS ActiveDoctors
FROM [dbo].[Doctors] d
INNER JOIN [dbo].[Specializations] s ON d.[SpecializationId] = s.[Id]
GROUP BY s.[Id], s.[Name]
ORDER BY s.[Id];
GO

-- Total active doctors count
SELECT COUNT(*) AS TotalActiveDoctors FROM [dbo].[Doctors] WHERE [del_flg] = 0;
GO

-- Count doctors per specialization (only active)
SELECT 
    s.[Name] AS Specialization,
    COUNT(d.[Id]) AS ActiveDoctors
FROM [dbo].[Specializations] s
LEFT JOIN [dbo].[Doctors] d ON s.[Id] = d.[SpecializationId] AND d.[del_flg] = 0
WHERE s.[del_flg] = 0
GROUP BY s.[Id], s.[Name]
ORDER BY ActiveDoctors DESC, s.[Id];
GO
