namespace XuHos.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V100 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConversationFriends",
                c => new
                    {
                        FriendID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        ConversationRoomID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        FromUserIdentifier = c.Int(nullable: false),
                        ToUserIdentifier = c.Int(nullable: false),
                        GroupName = c.String(nullable: false, unicode: false),
                        Remark = c.String(unicode: false),
                        AddWording = c.String(unicode: false),
                        ToUserID = c.String(nullable: false, unicode: false),
                        FromUserID = c.String(nullable: false, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.FriendID);
            
            CreateTable(
                "dbo.ConversationIMUids",
                c => new
                    {
                        Identifier = c.Int(nullable: false, identity: true),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        Enable = c.Boolean(nullable: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.Identifier);
            
            CreateTable(
                "dbo.ConversationMessages",
                c => new
                    {
                        ConversationMessageID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ConversationRoomID = c.Int(nullable: false),
                        ServiceID = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        MessageType = c.String(nullable: false, unicode: false),
                        MessageContent = c.String(nullable: false, maxLength: 4000, storeType: "nvarchar"),
                        MessageTime = c.DateTime(nullable: false, precision: 0),
                        MessageState = c.Int(nullable: false),
                        MessageSeq = c.String(nullable: false, maxLength: 32, unicode: false),
                        MessageIndex = c.Int(nullable: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ConversationMessageID);
            
            CreateTable(
                "dbo.ConversationRecordings",
                c => new
                    {
                        FileID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ChannelID = c.String(maxLength: 32, storeType: "nvarchar"),
                        FileURL = c.String(nullable: false, maxLength: 256, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.FileID);
            
            CreateTable(
                "dbo.ConversationRoomLogs",
                c => new
                    {
                        ConversationRoomLogID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ConversationRoomID = c.String(nullable: false, maxLength: 32, unicode: false),
                        OperationUserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        OperatorUserName = c.String(nullable: false, maxLength: 32, unicode: false),
                        OperatorType = c.String(nullable: false, maxLength: 32, unicode: false),
                        OperationTime = c.DateTime(nullable: false, precision: 0),
                        OperationDesc = c.String(nullable: false, maxLength: 200, unicode: false),
                        OperationRemark = c.String(nullable: false, maxLength: 200, unicode: false),
                    })
                .PrimaryKey(t => t.ConversationRoomLogID);
            
            CreateTable(
                "dbo.ConversationRooms",
                c => new
                    {
                        ConversationRoomID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ServiceID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ServiceType = c.Int(nullable: false),
                        ChannelID = c.Int(nullable: false),
                        Secret = c.String(nullable: false, maxLength: 32, unicode: false),
                        RoomState = c.Int(nullable: false),
                        BeginTime = c.DateTime(nullable: false, precision: 0),
                        EndTime = c.DateTime(nullable: false, precision: 0),
                        TotalTime = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        Enable = c.Boolean(nullable: false),
                        DisableWebSdkInteroperability = c.Boolean(nullable: false),
                        Close = c.Boolean(nullable: false),
                        RoomType = c.Int(nullable: false),
                        TriageID = c.Long(nullable: false),
                        Priority = c.Int(nullable: false),
                        ChargingState = c.Int(nullable: false),
                        ChargingSeq = c.Int(nullable: false),
                        ChargingTime = c.DateTime(nullable: false, precision: 0),
                        ChargingInterval = c.Int(nullable: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ConversationRoomID);
            
            CreateTable(
                "dbo.ConversationRoomUids",
                c => new
                    {
                        ConversationRoomID = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Identifier = c.Int(nullable: false),
                        UserType = c.Int(nullable: false),
                        ChannelID = c.Int(nullable: false),
                        UserMemberID = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserCNName = c.String(nullable: false, maxLength: 64, unicode: false),
                        UserENName = c.String(nullable: false, maxLength: 64, unicode: false),
                        PhotoUrl = c.String(nullable: false, maxLength: 200, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ConversationRoomID);
            
            CreateTable(
                "dbo.DoctorMembers",
                c => new
                    {
                        DoctorMemberID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DoctorID = c.String(nullable: false, maxLength: 32, unicode: false),
                        MemberID = c.String(nullable: false, maxLength: 32, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.DoctorMemberID);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DoctorName = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        Gender = c.Int(nullable: false),
                        Marriage = c.Int(nullable: false),
                        Birthday = c.String(nullable: false, maxLength: 10, unicode: false),
                        IDType = c.Int(nullable: false),
                        IDNumber = c.String(nullable: false, maxLength: 32, unicode: false),
                        Address = c.String(maxLength: 256, storeType: "nvarchar"),
                        PostCode = c.String(maxLength: 6, unicode: false),
                        Intro = c.String(nullable: false, maxLength: 4000, storeType: "nvarchar"),
                        IsExpert = c.Boolean(nullable: false, storeType: "bit"),
                        Specialty = c.String(nullable: false, maxLength: 1024, storeType: "nvarchar"),
                        areaCode = c.String(nullable: false, maxLength: 16, unicode: false),
                        HospitalID = c.String(nullable: false, maxLength: 32, unicode: false),
                        HospitalName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Grade = c.String(nullable: false, maxLength: 32, unicode: false),
                        DepartmentID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DepartmentName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Education = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        Title = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        Duties = c.String(maxLength: 512, storeType: "nvarchar"),
                        CheckState = c.Int(nullable: false),
                        SignatureURL = c.String(maxLength: 128, unicode: false),
                        BJCASignature = c.String(maxLength: 4000, unicode: false),
                        CertificateNo = c.String(maxLength: 64, unicode: false),
                        OpenID = c.String(maxLength: 50, unicode: false),
                        IsShow = c.Boolean(nullable: false),
                        DoctorType = c.Int(nullable: false),
                        DiseaseLabel = c.String(maxLength: 512, storeType: "nvarchar"),
                        Sort = c.Int(nullable: false),
                        RoomName = c.String(maxLength: 50, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.DoctorID);
            
            CreateTable(
                "dbo.DoctorSchedules",
                c => new
                    {
                        ScheduleID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DoctorID = c.String(nullable: false, maxLength: 32, unicode: false),
                        OPDate = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        StartTime = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        EndTime = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        Number = c.Int(nullable: false),
                        AppointNumber = c.Int(nullable: false),
                        NumberSourceDetailID = c.String(unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ScheduleID);
            
            CreateTable(
                "dbo.DoctorServices",
                c => new
                    {
                        ServiceID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DoctorID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ServiceType = c.Int(nullable: false),
                        ServiceSwitch = c.Boolean(nullable: false, storeType: "bit"),
                        ServicePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ServiceID);
            
            CreateTable(
                "dbo.HospitalDepartments",
                c => new
                    {
                        DepartmentID = c.String(nullable: false, maxLength: 32, unicode: false),
                        HospitalID = c.String(maxLength: 32, unicode: false),
                        DepartmentName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Intro = c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"),
                        CAT_NO = c.String(maxLength: 50, unicode: false),
                        PARENT_CAT_NO = c.String(maxLength: 50, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Hospitals",
                c => new
                    {
                        HospitalID = c.String(nullable: false, maxLength: 32, unicode: false),
                        HospitalName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Intro = c.String(nullable: false, maxLength: 4000, storeType: "nvarchar"),
                        License = c.String(nullable: false, maxLength: 32, unicode: false),
                        LogoUrl = c.String(nullable: false, maxLength: 128, unicode: false),
                        Address = c.String(nullable: false, maxLength: 256, storeType: "nvarchar"),
                        AreaName = c.String(maxLength: 10, storeType: "nvarchar"),
                        DrugstoreManageName = c.String(maxLength: 32, storeType: "nvarchar"),
                        Mobile = c.String(maxLength: 16, unicode: false),
                        Longitude = c.String(maxLength: 30, unicode: false),
                        Latitude = c.String(maxLength: 30, unicode: false),
                        PostCode = c.String(nullable: false, maxLength: 6, unicode: false),
                        Telephone = c.String(nullable: false, maxLength: 32, unicode: false),
                        Email = c.String(nullable: false, maxLength: 64, unicode: false),
                        OrgType = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Path = c.String(maxLength: 256, unicode: false),
                        ParentID = c.String(maxLength: 32, unicode: false),
                        ImageUrl = c.String(nullable: false, maxLength: 128, unicode: false),
                        ListImageUrl = c.String(maxLength: 128, unicode: false),
                        IsShowInWeb = c.Boolean(nullable: false),
                        IsStandalone = c.Boolean(nullable: false),
                        IsCooperation = c.Boolean(nullable: false),
                        IsUseWisdom = c.Boolean(nullable: false),
                        Mp4Url = c.String(maxLength: 128, unicode: false),
                        Mp4PreviewUrl = c.String(maxLength: 128, unicode: false),
                        ChannelID = c.String(maxLength: 50, unicode: false),
                        HomePageTheme = c.String(maxLength: 128, unicode: false),
                        ProvinceID = c.String(maxLength: 32, unicode: false),
                        CityID = c.String(maxLength: 32, unicode: false),
                        AreaID = c.String(maxLength: 32, unicode: false),
                        IsUseParentOrgDrug = c.Boolean(nullable: false, storeType: "bit"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.HospitalID);
            
            CreateTable(
                "dbo.HospitalWorkingTimes",
                c => new
                    {
                        WorkingTimeID = c.String(nullable: false, maxLength: 32, unicode: false),
                        HospitalID = c.String(nullable: false, maxLength: 32, unicode: false),
                        StartTime = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        EndTime = c.String(nullable: false, maxLength: 16, storeType: "nvarchar"),
                        Sort = c.Int(nullable: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.WorkingTimeID);
            
            CreateTable(
                "dbo.OrderCallbackLogs",
                c => new
                    {
                        CallbackLogID = c.String(nullable: false, maxLength: 64, unicode: false),
                        OrderNo = c.String(nullable: false, maxLength: 64, unicode: false),
                        OrgID = c.String(nullable: false, maxLength: 32, unicode: false),
                        Status = c.Int(nullable: false),
                        TriedTimes = c.Int(nullable: false),
                        Message = c.String(maxLength: 1000, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.CallbackLogID);
            
            CreateTable(
                "dbo.OrderConsignees",
                c => new
                    {
                        OrderNo = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        ConsigneeAddress = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        ConsigneeName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        ConsigneeTel = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        SendGoodsTime = c.DateTime(precision: 0),
                        IsHosAddress = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.OrderNo);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        OrderNO = c.String(maxLength: 32, storeType: "nvarchar"),
                        ProductId = c.String(nullable: false, maxLength: 32, unicode: false),
                        Subject = c.String(nullable: false, unicode: false),
                        Body = c.String(nullable: false, unicode: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(unicode: false),
                        GroupNo = c.Int(nullable: false),
                        ProductType = c.Int(nullable: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.OrderDetailID);
            
            CreateTable(
                "dbo.OrderDiscounts",
                c => new
                    {
                        OrderDiscountID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        OrderNo = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Privilege = c.Int(nullable: false),
                        DiscountPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        State = c.Int(nullable: false),
                        PrivilegeOutID = c.String(maxLength: 32, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.OrderDiscountID);
            
            CreateTable(
                "dbo.OrderLogs",
                c => new
                    {
                        OrderLogID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        OrderNo = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        OperationType = c.Int(nullable: false),
                        OperationDesc = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Remark = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        OperationTime = c.DateTime(nullable: false, precision: 0),
                        OperationUserID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.OrderLogID);
            
            CreateTable(
                "dbo.OrderRefundLogs",
                c => new
                    {
                        RefundLog = c.String(nullable: false, maxLength: 32, unicode: false),
                        OrderNo = c.String(nullable: false, maxLength: 32, unicode: false),
                        PayType = c.Int(nullable: false),
                        TradeNo = c.String(nullable: false, maxLength: 32, unicode: false),
                        RefundNo = c.String(nullable: false, maxLength: 64, unicode: false),
                        RefundFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefundState = c.Int(nullable: false),
                        RefundTime = c.DateTime(nullable: false, precision: 0),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.RefundLog);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderNo = c.String(nullable: false, maxLength: 64, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        OrderOutID = c.String(nullable: false, maxLength: 64, unicode: false),
                        OrderState = c.Int(nullable: false),
                        OrderType = c.Int(nullable: false),
                        CostType = c.Int(nullable: false),
                        RefundFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefundNo = c.String(nullable: false, maxLength: 64, unicode: false),
                        RefundState = c.Int(nullable: false),
                        RefundTime = c.DateTime(nullable: false, precision: 0),
                        PayType = c.Int(nullable: false),
                        TradeNo = c.String(nullable: false, maxLength: 64, unicode: false),
                        TradeTime = c.DateTime(nullable: false, precision: 0),
                        StoreTime = c.DateTime(nullable: false, precision: 0),
                        ExpressTime = c.DateTime(nullable: false, precision: 0),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        totalFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderTime = c.DateTime(nullable: false, precision: 0),
                        OrderExpireTime = c.DateTime(nullable: false, precision: 0),
                        CancelTime = c.DateTime(nullable: false, precision: 0),
                        FinishTime = c.DateTime(nullable: false, precision: 0),
                        CancelReason = c.String(nullable: false, maxLength: 1024, storeType: "nvarchar"),
                        LogisticNo = c.String(maxLength: 32, storeType: "nvarchar"),
                        LogisticState = c.Int(nullable: false),
                        LogisticCompanyName = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        LogisticWayBillNo = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        SellerID = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        OrgnazitionID = c.String(maxLength: 32, unicode: false),
                        IsEvaluated = c.Boolean(nullable: false, storeType: "bit"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.OrderNo);
            
            CreateTable(
                "dbo.OrderTradeLogs",
                c => new
                    {
                        TradeLog = c.String(nullable: false, maxLength: 32, unicode: false),
                        OrderNo = c.String(nullable: false, maxLength: 32, unicode: false),
                        PayType = c.Int(nullable: false),
                        TradeNo = c.String(nullable: false, maxLength: 32, unicode: false),
                        TradeTime = c.DateTime(nullable: false, precision: 0),
                        TradeFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SellerID = c.String(unicode: false),
                        TradeStatus = c.Int(nullable: false),
                        OnlineTransactionNo = c.String(unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.TradeLog);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        RegionID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ParentID = c.String(maxLength: 32, unicode: false),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Level = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        HotLevel = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.RegionID);
            
            CreateTable(
                "dbo.ServiceEvaluations",
                c => new
                    {
                        ServiceEvaluationID = c.String(nullable: false, maxLength: 32, unicode: false),
                        OuterID = c.String(nullable: false, maxLength: 32, unicode: false),
                        Score = c.Int(nullable: false),
                        EvaluationTags = c.String(nullable: false, maxLength: 512, storeType: "nvarchar"),
                        Content = c.String(maxLength: 1024, storeType: "nvarchar"),
                        ProviderID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ServiceType = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ServiceEvaluationID);
            
            CreateTable(
                "dbo.ServiceEvaluationTags",
                c => new
                    {
                        ServiceEvaluationTagID = c.String(nullable: false, maxLength: 32, unicode: false),
                        Score = c.Int(nullable: false),
                        TagName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ServiceEvaluationTagID);
            
            CreateTable(
                "dbo.SysAccessAccounts",
                c => new
                    {
                        AccessID = c.String(nullable: false, maxLength: 32, unicode: false),
                        AppId = c.String(nullable: false, maxLength: 512, unicode: false),
                        AppSecret = c.String(nullable: false, maxLength: 512, unicode: false),
                        AppKey = c.String(nullable: false, maxLength: 512, unicode: false),
                        SourceType = c.String(nullable: false, maxLength: 50, unicode: false),
                        UserKey = c.String(maxLength: 50, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.AccessID);
            
            CreateTable(
                "dbo.SysConfigs",
                c => new
                    {
                        ConfigKey = c.String(nullable: false, maxLength: 32, unicode: false),
                        ConfigValue = c.String(nullable: false, maxLength: 512, unicode: false),
                        Remark = c.String(nullable: false, maxLength: 512, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ConfigKey);
            
            CreateTable(
                "dbo.SysDereplications",
                c => new
                    {
                        SysDereplicationID = c.String(nullable: false, maxLength: 32, unicode: false),
                        TableName = c.String(maxLength: 64, unicode: false),
                        OutID = c.String(maxLength: 32, unicode: false),
                        DereplicationType = c.Int(nullable: false),
                        SuccessCount = c.Int(nullable: false),
                        FailCount = c.Int(nullable: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.SysDereplicationID);
            
            CreateTable(
                "dbo.SysDicts",
                c => new
                    {
                        DicID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DictTypeID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DicCode = c.String(nullable: false, maxLength: 32, unicode: false),
                        CNName = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        ENName = c.String(nullable: false, maxLength: 64, unicode: false),
                        OrderNo = c.Int(nullable: false),
                        Remark = c.String(nullable: false, maxLength: 512, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.DicID);
            
            CreateTable(
                "dbo.SysEventConsumes",
                c => new
                    {
                        ConsumeID = c.String(nullable: false, maxLength: 32, unicode: false),
                        EventID = c.String(nullable: false, maxLength: 32, unicode: false),
                        QueueName = c.String(nullable: false, maxLength: 1000, unicode: false),
                        RetryCount = c.Int(nullable: false),
                        Finished = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ConsumeID);
            
            CreateTable(
                "dbo.SysEvents",
                c => new
                    {
                        EventID = c.String(nullable: false, maxLength: 32, unicode: false),
                        RouteKey = c.String(nullable: false, maxLength: 1000, unicode: false),
                        EventObject = c.String(nullable: false, maxLength: 1000, unicode: false),
                        Priority = c.Int(nullable: false),
                        Enqueued = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.EventID);
            
            CreateTable(
                "dbo.SysFileIndexes",
                c => new
                    {
                        MD5 = c.String(nullable: false, maxLength: 32, unicode: false),
                        FileUrl = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        FileType = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Remark = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        ReadCount = c.Long(nullable: false),
                        FileSize = c.Long(nullable: false),
                        AccessKey = c.String(nullable: false, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.MD5);
            
            CreateTable(
                "dbo.SysModules",
                c => new
                    {
                        ModuleID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ModuleName = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        ModuleType = c.Int(nullable: false),
                        ModuleUrl = c.String(nullable: false, maxLength: 256, unicode: false),
                        ParentModuleID = c.String(maxLength: 32, unicode: false),
                        TopModuleID = c.String(maxLength: 32, unicode: false),
                        Level = c.Int(nullable: false),
                        Sort = c.Int(nullable: false),
                        CSSClass = c.String(maxLength: 1000, unicode: false),
                        Target = c.String(maxLength: 1000, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ModuleID);
            
            CreateTable(
                "dbo.SysMonitorIndexes",
                c => new
                    {
                        SysMonitorID = c.String(nullable: false, maxLength: 32, unicode: false),
                        OutID = c.String(nullable: false, maxLength: 32, unicode: false),
                        Category = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Value = c.String(nullable: false, maxLength: 1000, unicode: false),
                        ModifyTime = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.SysMonitorID);
            
            CreateTable(
                "dbo.SysShortMessageTemplates",
                c => new
                    {
                        TemplateID = c.String(nullable: false, maxLength: 32, unicode: false),
                        TemplateContent = c.String(nullable: false, maxLength: 512, unicode: false),
                        Remark = c.String(nullable: false, maxLength: 512, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.TemplateID);
            
            CreateTable(
                "dbo.UserExtends",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        LastTime = c.DateTime(nullable: false, precision: 0),
                        JRegisterID = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.UserFiles",
                c => new
                    {
                        FileID = c.String(nullable: false, maxLength: 32, unicode: false),
                        OutID = c.String(nullable: false, maxLength: 32, unicode: false),
                        FileName = c.String(nullable: false, maxLength: 128, unicode: false),
                        FileUrl = c.String(nullable: false, maxLength: 128, unicode: false),
                        FileType = c.Int(nullable: false),
                        Remark = c.String(nullable: false, maxLength: 512, storeType: "nvarchar"),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        AccessKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        ResourceID = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.FileID);
            
            CreateTable(
                "dbo.UserLoginLogs",
                c => new
                    {
                        UserLoginID = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        LoginAccount = c.String(nullable: false, maxLength: 32, unicode: false),
                        OrgID = c.String(maxLength: 32, unicode: false),
                        LoginType = c.Int(nullable: false),
                        LoginTime = c.DateTime(nullable: false, precision: 0),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.UserLoginID);
            
            CreateTable(
                "dbo.UserMedicalRecords",
                c => new
                    {
                        UserMedicalRecordID = c.String(nullable: false, maxLength: 32, unicode: false),
                        OPDRegisterID = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        MemberID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DoctorID = c.String(nullable: false, maxLength: 32, unicode: false),
                        Sympton = c.String(maxLength: 1000, storeType: "nvarchar"),
                        PastMedicalHistory = c.String(maxLength: 1000, storeType: "nvarchar"),
                        PresentHistoryIllness = c.String(maxLength: 1000, storeType: "nvarchar"),
                        PreliminaryDiagnosis = c.String(maxLength: 4000, storeType: "nvarchar"),
                        ConsultationSubject = c.String(maxLength: 1000, storeType: "nvarchar"),
                        Description = c.String(maxLength: 1000, storeType: "nvarchar"),
                        PastOperatedHistory = c.String(maxLength: 4000, storeType: "nvarchar"),
                        FamilyMedicalHistory = c.String(maxLength: 1000, storeType: "nvarchar"),
                        AllergicHistory = c.String(maxLength: 1000, storeType: "nvarchar"),
                        IndividualHistory = c.String(maxLength: 1000, storeType: "nvarchar"),
                        LifeHistory = c.String(maxLength: 1000, storeType: "nvarchar"),
                        ObstetricalHistory = c.String(maxLength: 1000, storeType: "nvarchar"),
                        MenstrualHistory = c.String(maxLength: 1000, storeType: "nvarchar"),
                        Advised = c.String(maxLength: 1000, storeType: "nvarchar"),
                        OrgnazitionID = c.String(maxLength: 32, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.UserMedicalRecordID);
            
            CreateTable(
                "dbo.UserMemberEMRs",
                c => new
                    {
                        UserMemberEMRID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        MemberID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        EMRName = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        Date = c.DateTime(nullable: false, precision: 0),
                        HospitalName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Remark = c.String(maxLength: 1024, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.UserMemberEMRID);
            
            CreateTable(
                "dbo.UserMembers",
                c => new
                    {
                        MemberID = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserID = c.String(maxLength: 32, unicode: false),
                        MemberName = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Relation = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        Marriage = c.Int(nullable: false),
                        Birthday = c.String(nullable: false, maxLength: 10, unicode: false),
                        Mobile = c.String(maxLength: 20, unicode: false),
                        IDType = c.Int(nullable: false),
                        IDNumber = c.String(maxLength: 32, unicode: false),
                        Nationality = c.String(maxLength: 50, storeType: "nvarchar"),
                        Province = c.String(maxLength: 50, storeType: "nvarchar"),
                        ProvinceRegionID = c.String(maxLength: 32, unicode: false),
                        City = c.String(maxLength: 50, storeType: "nvarchar"),
                        CityRegionID = c.String(maxLength: 32, unicode: false),
                        District = c.String(maxLength: 50, storeType: "nvarchar"),
                        DistrictRegionID = c.String(maxLength: 32, unicode: false),
                        Town = c.String(maxLength: 50, storeType: "nvarchar"),
                        TownRegionID = c.String(maxLength: 32, unicode: false),
                        Village = c.String(maxLength: 50, storeType: "nvarchar"),
                        VillageRegionID = c.String(maxLength: 32, unicode: false),
                        IsAllergic = c.Boolean(nullable: false),
                        AllergicRemark = c.String(maxLength: 50, storeType: "nvarchar"),
                        Address = c.String(maxLength: 256, storeType: "nvarchar"),
                        Email = c.String(maxLength: 64, unicode: false),
                        PostCode = c.String(maxLength: 6, unicode: false),
                        IsDefault = c.Boolean(nullable: false, storeType: "bit"),
                        Remark = c.String(maxLength: 500, storeType: "nvarchar"),
                        Ethnic = c.String(maxLength: 30, storeType: "nvarchar"),
                        Occupation = c.String(maxLength: 100, storeType: "nvarchar"),
                        CompanyName = c.String(maxLength: 100, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.MemberID);
            
            CreateTable(
                "dbo.UserOPDRegisters",
                c => new
                    {
                        OPDRegisterID = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DoctorID = c.String(nullable: false, maxLength: 32, unicode: false),
                        DoctorGroupID = c.String(nullable: false, maxLength: 32, unicode: false),
                        ScheduleID = c.String(maxLength: 32, unicode: false),
                        RegDate = c.DateTime(nullable: false, precision: 0),
                        OPDDate = c.DateTime(nullable: false, precision: 0),
                        OPDBeginTime = c.String(nullable: false, unicode: false),
                        OPDEndTime = c.String(nullable: false, unicode: false),
                        ConsultContent = c.String(nullable: false, maxLength: 400, storeType: "nvarchar"),
                        ConsultDisease = c.String(maxLength: 128, storeType: "nvarchar"),
                        OPDType = c.Int(nullable: false),
                        Fee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MemberID = c.String(nullable: false, unicode: false),
                        MemberName = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        Gender = c.Int(nullable: false),
                        Marriage = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        IDNumber = c.String(nullable: false, maxLength: 32, unicode: false),
                        IDType = c.Int(nullable: false),
                        Mobile = c.String(maxLength: 20, unicode: false),
                        Address = c.String(maxLength: 256, storeType: "nvarchar"),
                        Birthday = c.String(nullable: false, maxLength: 10, unicode: false),
                        OrgnazitionID = c.String(nullable: false, maxLength: 32, unicode: false),
                        IsUseTaskPool = c.Boolean(nullable: false),
                        Flag = c.Int(nullable: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.OPDRegisterID);
            
            CreateTable(
                "dbo.UserRoleMaps",
                c => new
                    {
                        UserRoleMapID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        RoleID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        UserID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.UserRoleMapID);
            
            CreateTable(
                "dbo.UserRolePrevileges",
                c => new
                    {
                        PrevilegeID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        RoleID = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                        ModuleID = c.String(nullable: false, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.PrevilegeID);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        RoleID = c.String(nullable: false, maxLength: 32, unicode: false),
                        RoleName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Memo = c.String(maxLength: 200, storeType: "nvarchar"),
                        RoleType = c.Int(nullable: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserAccount = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserCNName = c.String(maxLength: 64, storeType: "nvarchar"),
                        UserENName = c.String(maxLength: 32, unicode: false),
                        UserType = c.Int(nullable: false),
                        Mobile = c.String(nullable: false, maxLength: 16, unicode: false),
                        Email = c.String(maxLength: 64, unicode: false),
                        Password = c.String(nullable: false, maxLength: 64, unicode: false),
                        PayPassword = c.String(maxLength: 32, unicode: false),
                        PhotoUrl = c.String(maxLength: 128, unicode: false),
                        RegTime = c.DateTime(nullable: false, precision: 0),
                        CancelTime = c.DateTime(nullable: false, precision: 0),
                        UserState = c.Int(nullable: false),
                        UserLevel = c.Int(nullable: false),
                        IsTestAccount = c.Boolean(nullable: false),
                        TestEndDate = c.DateTime(precision: 0),
                        Terminal = c.String(nullable: false, maxLength: 50, unicode: false),
                        OrgCode = c.String(maxLength: 64, unicode: false),
                        RegisterType = c.Int(nullable: false),
                        ExpiredTime = c.DateTime(precision: 0),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.UserShortMessageLogs",
                c => new
                    {
                        ShortMessageLogID = c.String(nullable: false, maxLength: 32, unicode: false),
                        MsgLogType = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 32, unicode: false),
                        TelePhoneNum = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                        MsgTitle = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        MsgContent = c.String(nullable: false, maxLength: 500, storeType: "nvarchar"),
                        OutTime = c.DateTime(nullable: false, precision: 0),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.ShortMessageLogID);
            
            CreateTable(
                "dbo.UserWechatMaps",
                c => new
                    {
                        UserWechatMapID = c.String(nullable: false, maxLength: 32, unicode: false),
                        UserID = c.String(maxLength: 32, unicode: false),
                        OpenID = c.String(nullable: false, maxLength: 256, unicode: false),
                        AppID = c.String(nullable: false, maxLength: 256, unicode: false),
                        CreateUserID = c.String(maxLength: 32, unicode: false),
                        CreateTime = c.DateTime(nullable: false, precision: 0),
                        ModifyUserID = c.String(maxLength: 32, unicode: false),
                        ModifyTime = c.DateTime(precision: 0),
                        DeleteUserID = c.String(maxLength: 32, unicode: false),
                        DeleteTime = c.DateTime(precision: 0),
                        IsDeleted = c.Boolean(nullable: false, storeType: "bit"),
                    })
                .PrimaryKey(t => t.UserWechatMapID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserWechatMaps");
            DropTable("dbo.UserShortMessageLogs");
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserRolePrevileges");
            DropTable("dbo.UserRoleMaps");
            DropTable("dbo.UserOPDRegisters");
            DropTable("dbo.UserMembers");
            DropTable("dbo.UserMemberEMRs");
            DropTable("dbo.UserMedicalRecords");
            DropTable("dbo.UserLoginLogs");
            DropTable("dbo.UserFiles");
            DropTable("dbo.UserExtends");
            DropTable("dbo.SysShortMessageTemplates");
            DropTable("dbo.SysMonitorIndexes");
            DropTable("dbo.SysModules");
            DropTable("dbo.SysFileIndexes");
            DropTable("dbo.SysEvents");
            DropTable("dbo.SysEventConsumes");
            DropTable("dbo.SysDicts");
            DropTable("dbo.SysDereplications");
            DropTable("dbo.SysConfigs");
            DropTable("dbo.SysAccessAccounts");
            DropTable("dbo.ServiceEvaluationTags");
            DropTable("dbo.ServiceEvaluations");
            DropTable("dbo.Regions");
            DropTable("dbo.OrderTradeLogs");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderRefundLogs");
            DropTable("dbo.OrderLogs");
            DropTable("dbo.OrderDiscounts");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.OrderConsignees");
            DropTable("dbo.OrderCallbackLogs");
            DropTable("dbo.HospitalWorkingTimes");
            DropTable("dbo.Hospitals");
            DropTable("dbo.HospitalDepartments");
            DropTable("dbo.DoctorServices");
            DropTable("dbo.DoctorSchedules");
            DropTable("dbo.Doctors");
            DropTable("dbo.DoctorMembers");
            DropTable("dbo.ConversationRoomUids");
            DropTable("dbo.ConversationRooms");
            DropTable("dbo.ConversationRoomLogs");
            DropTable("dbo.ConversationRecordings");
            DropTable("dbo.ConversationMessages");
            DropTable("dbo.ConversationIMUids");
            DropTable("dbo.ConversationFriends");
        }
    }
}
