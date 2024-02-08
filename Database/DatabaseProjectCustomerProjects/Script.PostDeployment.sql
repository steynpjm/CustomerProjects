-- This file contains SQL statements that will be executed after the build script.



--drop table dbo.[PROJECT];
--drop table dbo.[USER];
--drop table dbo.[COMPANY];


delete dbo.[PROJECT];
delete dbo.[USER];
delete dbo.[COMPANY];

GO

DBCC CHECKIDENT ('[COMPANY]', RESEED, 0);
DBCC CHECKIDENT ('[USER]', RESEED, 0);
DBCC CHECKIDENT ('[PROJECT]', RESEED, 0);

GO



INSERT INTO [dbo].[COMPANY]([name], [address1], [address2], [town], [postalCode], [country])
VALUES
    ('THE Main Company', '111 Winner Street', 'Silicon Valley', 'Cape Town', '8000', 'South Africa');

GO


INSERT INTO [dbo].[USER](companyHID, userName, password, title, firstname, lastName, email, designation)
VALUES
    ((select top 1 id from dbo.COMPANY), 'steynpjm', 'vm1400', 'MR', 'Marius', 'Steyn', 'marius@mariussteyn.com', 'manager')

GO


INSERT INTO [dbo].[PROJECT](companyHID, name, description, code, managerHID)
VALUES
    ((select top 1 id from dbo.COMPANY), 'ASSESMENT', 'Create an assement project', 'A001', 1)



GO


