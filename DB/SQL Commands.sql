CREATE DATABASE WorkScheduleManagementSystemDb


USE WorkScheduleManagementSystemDb


CREATE TABLE Employees
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(15) NOT NULL,
    LastName NVARCHAR(15) NOT NULL,
    Email NVARCHAR(max) NOT NULL UNIQUE,
    Phone NVARCHAR(15) NOT NULL UNIQUE,
    HireDate DATE DEFAULT GETDATE(),
    Salary DECIMAL(10,2)
)


CREATE TABLE AttendanceAndDepartureTimes
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    AttendanceTime DATETIME NOT NULL,
    DepartureTime DATETIME,
    EmployeeId INT REFERENCES Employees(Id) NOT NULL
)


-- =========================================
-- 1. Insert Data into Employees Table
-- =========================================
INSERT INTO Employees
    (FirstName, LastName, Email, Phone, HireDate, Salary)
VALUES
    (N'John', N'Doe', N'john.doe@company.com', N'555-010-1001', '2023-01-15', 65000.00),
    (N'Jane', N'Smith', N'jane.smith@company.com', N'555-010-1002', '2022-06-20', 78000.00),
    (N'Michael', N'Johnson', N'm.johnson@company.com', N'555-010-1003', '2024-03-10', 55000.00),
    (N'Emily', N'Davis', N'emily.davis@company.com', N'555-010-1004', '2025-11-01', 82000.00),
    (N'David', N'Wilson', N'd.wilson@company.com', N'555-010-1005', '2026-02-14', 60000.00);

-- =========================================
-- 2. Insert Data into Attendance Table
-- =========================================
-- Note: This assumes the Employees were inserted into an empty table, 
-- so their IDs are 1, 2, 3, 4, and 5 respectively.
INSERT INTO AttendanceAndDepartureTimes
    (AttendanceTime, DepartureTime, EmployeeId)
VALUES
    -- Yesterday's attendance (August 25, 2026)
    ('2026-08-25 08:00:00', '2026-08-25 01:05:00', 1),
    ('2026-08-25 08:15:00', '2026-08-25 01:30:00', 2),
    ('2026-08-25 07:50:00', '2026-08-25 01:45:00', 3),
    ('2026-08-25 09:10:00', '2026-08-25 02:00:00', 4),
    ('2026-08-25 08:05:00', '2026-08-25 01:10:00', 5),

    -- Today's attendance (August 26, 2026)
    ('2026-08-26 08:02:00', '2026-08-26 02:00:00', 1),
    ('2026-08-26 08:20:00', '2026-08-26 01:25:00', 2),
    ('2026-08-26 07:55:00', '2026-08-26 01:50:00', 3),
    ('2026-08-26 09:00:00', '2026-08-26 01:15:00', 4);
-- Note: Employee 5 hasn't clocked in today yet, which is why they are omitted from today's records.

-- =========================================
-- 3. Verify the Inserted Data
-- =========================================
SELECT
    AD.Id
    E.Id AS EmployeeId,
    E.FirstName + ' ' + E.LastName AS FullName,
    AD.AttendanceTime,
    AD.DepartureTime
FROM AttendanceAndDepartureTimes AD
    INNER JOIN Employees E ON E.Id = AD.EmployeeId
        WHERE EmployeeId=1 AND YEAR(AD.AttendanceTime)=2026 AND MONTH(AD.AttendanceTime)=8
ORDER BY AD.AttendanceTime
/*OFFSET (1-1)*2 ROWS
FETCH NEXT 2 ROWS ONLY*/


GO
CREATE PROCEDURE AddEmployee
    @FirstName NVARCHAR(15),
    @LastName NVARCHAR(15),
    @Email NVARCHAR(max),
    @Phone NVARCHAR(15),
    @Salary DECIMAL(10,2)
AS
BEGIN TRY 
    INSERT INTO Employees(FirstName, LastName, Email, Phone, Salary)
    VALUES(@FirstName, @LastName, @Email, @Phone, @Salary)
END TRY
BEGIN CATCH
    SELECT 'Failed to add'
END CATCH


GO
CREATE PROCEDURE UpdateEmployee
    @Id INT,
    @FirstName NVARCHAR(15),
    @LastName NVARCHAR(15),
    @Email NVARCHAR(max),
    @Phone NVARCHAR(15),
    @Salary DECIMAL(10,2)
AS
BEGIN TRY 
    UPDATE Employees 
    SET FirstName = @FirstName,
        LastName=@LastName,
        Email=@Email,
        Phone=@Phone,
        Salary=@Salary
    WHERE Id=@Id
END TRY
BEGIN CATCH
    SELECT 'Failed to update'
END CATCH


GO
CREATE PROCEDURE AddAttendanceTime
    @AttendanceTime DATETIME,
    @EmployeeId INT
AS
BEGIN TRY 
    INSERT INTO AttendanceAndDepartureTimes(AttendanceTime,EmployeeId)
    VALUES(@AttendanceTime, @EmployeeId)
END TRY
BEGIN CATCH
    SELECT 'Failed to add'
END CATCH


GO
CREATE PROCEDURE AddDepartureTime
    @Id INT,
    @DepartureTime DATETIME,
    @EmployeeId INT
AS
BEGIN TRY 
    UPDATE AttendanceAndDepartureTimes 
    SET DepartureTime = @DepartureTime
    WHERE Id=@Id AND EmployeeId=@EmployeeId
END TRY
BEGIN CATCH
    SELECT 'Failed to add'
END CATCH


GO
CREATE VIEW MonthlyEmployeeReports
AS
    SELECT AD.EmployeeId, E.FirstName + ' ' + E.LastName AS FullName,
        SUM(DATEDIFF(MINUTE,DATEADD(HOUR,8,CAST(CAST(AD.AttendanceTime AS DATE) AS datetime)),AD.AttendanceTime))
        AS TotalLateAttendanceMinutes,
        SUM(DATEDIFF(MINUTE,AD.DepartureTime,DATEADD(HOUR,2,CAST(CAST(AD.DepartureTime AS DATE) AS datetime))))
        AS TotalEarlyDepartureMinutes,
        E.Salary
        -
        (E.Salary
        *
        ((SUM(DATEDIFF(MINUTE,DATEADD(HOUR,8,CAST(CAST(AD.AttendanceTime AS DATE) AS datetime)),AD.AttendanceTime))
          +
          SUM(DATEDIFF(MINUTE,AD.DepartureTime,DATEADD(HOUR,2,CAST(CAST(AD.DepartureTime AS DATE) AS datetime))))
         )
         *0.1)
        /
        100)
        AS TotalSalaryAfterDeductions,
        MONTH(AD.AttendanceTime) AS Month,
        YEAR(AD.AttendanceTime) AS Year
        FROM AttendanceAndDepartureTimes AS AD INNER JOIN Employees AS E
        ON E.Id=AD.EmployeeId
    GROUP BY YEAR(AD.AttendanceTime),MONTH(AD.AttendanceTime),AD.EmployeeId,E.FirstName, E.LastName,E.Salary



/*SELECT * FROM MonthlyEmployeeReports 
ORDER BY Year,Month*/