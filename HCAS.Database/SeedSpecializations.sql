-- Specialization Data Generation Script
-- This script generates sample specialization data for the HCAS database
-- Execute this script in SQL Server Management Studio or via sqlcmd
-- Note: IDs 1, 2, 3 already exist in the database - this script inserts new ones (4-20)

SET IDENTITY_INSERT [dbo].[Specializations] ON
GO

-- Insert new specialization records (IDs 4-20)
-- Note: IDs 1, 2, 3 already exist - continuing from ID 4
INSERT [dbo].[Specializations] ([Id], [Name], [del_flg]) VALUES 
(4, N'အာရုံကြောအထူးကု', 0),
(5, N'အရိုးအထူးကု', 0),
(6, N'အရေပြားအထူးကု', 0),
(7, N'မျက်စိအထူးကု', 0),
(8, N'နားနှာခေါင်းလည်ချောင်းအထူးကု', 0),
(9, N'သူနာပြုဆေးခန်း', 0),
(10, N'စိတ်ရောဂါကုသဆရာဝန်', 0),
(11, N'ကလေးအထူးကု', 0),
(12, N'သားဖွားမီးယပ်အထူးကု', 0),
(13, N'ဆီးကျိတ်အထူးကု', 0),
(14, N'အဆုတ်အထူးကု', 0),
(15, N'သွေးကင်ဆာအထူးကု', 0),
(16, N'အနာကုဆရာဝန်', 0),
(17, N'အရေးပေါ်ဆေးကုဆရာဝန်', 0),
(18, N'ရောင်ရမ်းနာကျင်မှုအထူးကု', 0),
(19, N'ဆေးဝါးအထူးကု', 0),
(20, N'ကင်ဆာအထူးကု', 0);

SET IDENTITY_INSERT [dbo].[Specializations] OFF
GO

SET IDENTITY_INSERT [dbo].[Specializations] OFF
GO

-- Verify inserted records
SELECT COUNT(*) AS TotalSpecializations FROM [dbo].[Specializations] WHERE [del_flg] = 0;
GO

-- List all specializations (including existing ones)
SELECT [Id], [Name], [del_flg] AS IsDeleted 
FROM [dbo].[Specializations] 
WHERE [del_flg] = 0
ORDER BY [Id];
GO
