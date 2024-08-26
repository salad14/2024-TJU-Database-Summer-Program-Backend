-- 用户表
CREATE TABLE Users (
    user_id VARCHAR2(10) PRIMARY KEY,
    username VARCHAR2(50) UNIQUE NOT NULL,
    password VARCHAR2(255) NOT NULL,
    phone_number VARCHAR2(15),
    user_type VARCHAR2(20) CHECK(user_type IN ('普通用户', '管理员')),
    reservation_privileges CHAR(1) CHECK(reservation_privileges IN ('Y', 'N')),
    violation_count INT DEFAULT 0,
    vip_status CHAR(1) CHECK(vip_status IN ('Y', 'N')),
    registration_time DATE,
    discount_rate DECIMAL(5, 2)
);

-- 设置 registration_time 的默认值为当前日期和时间的触发器
CREATE TRIGGER set_registration_time
BEFORE INSERT ON Users
FOR EACH ROW
BEGIN
    IF :NEW.registration_time IS NULL THEN
        :NEW.registration_time := SYSDATE;
    END IF;
END;
/

-- 团体表
CREATE TABLE Groups (
    group_id VARCHAR2(10) PRIMARY KEY,
    group_name VARCHAR2(100) NOT NULL,
    group_description CLOB,
    member_count INT DEFAULT 0,
    creation_time DATE DEFAULT SYSDATE
);

-- 场地表
CREATE TABLE Venues (
    venue_id VARCHAR2(10) PRIMARY KEY,
    venue_name VARCHAR2(100),
    venue_type VARCHAR2(50),
    sport_type VARCHAR2(50), -- 运动类型
    location VARCHAR2(255), -- 位置
    image_url VARCHAR2(255), -- 场地图片
    capacity INT,
    status VARCHAR2(20),
    last_inspection DATE
);

-- 场地开放时间表
CREATE TABLE VenueAvailability (
    availability_id VARCHAR2(10) PRIMARY KEY,
    venue_id VARCHAR2(10),
    start_time TIMESTAMP,
    end_time TIMESTAMP,
    price DECIMAL(10, 2),
    remaining_capacity INT, -- 剩余容量
    CONSTRAINT fk_venue FOREIGN KEY (venue_id) REFERENCES Venues(venue_id)
);

-- 预约记录表
CREATE TABLE Reservations (
    reservation_id VARCHAR2(10) PRIMARY KEY,
    venue_id VARCHAR2(10),
    availability_id VARCHAR2(10),
    reservation_item VARCHAR2(100),
    reservation_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    start_time TIMESTAMP,
    end_time TIMESTAMP,
    payment_amount DECIMAL(10, 2),
    num_of_people INT, -- 预约人数
    CONSTRAINT fk_venue_reservation FOREIGN KEY (venue_id) REFERENCES Venues(venue_id),
    CONSTRAINT fk_availability FOREIGN KEY (availability_id) REFERENCES VenueAvailability(availability_id)
);

-- 团体预约成员记录表
CREATE TABLE GroupReservationMembers (
    reservation_id VARCHAR2(10),
    group_id VARCHAR2(10),
    num_of_people INT, -- 预约人数
    actual_num_of_people INT, -- 实到人数
    status VARCHAR2(50), -- 预约状态
    PRIMARY KEY (reservation_id, group_id),
    CONSTRAINT fk_reservation FOREIGN KEY (reservation_id) REFERENCES Reservations(reservation_id),
    CONSTRAINT fk_group FOREIGN KEY (group_id) REFERENCES Groups(group_id)
);

-- 场地保养记录表
CREATE TABLE VenueMaintenance (
    maintenance_id VARCHAR2(10) PRIMARY KEY,
    venue_id VARCHAR2(10),
    start_time TIMESTAMP, -- 保养开始时间
    end_time TIMESTAMP, -- 保养结束时间
    description CLOB,
    CONSTRAINT fk_venue_maintenance FOREIGN KEY (venue_id) REFERENCES Venues(venue_id)
);

-- 设备表
CREATE TABLE Equipments (
    equipment_id VARCHAR2(10) PRIMARY KEY,
    admin_id VARCHAR2(10),
    equipment_name VARCHAR2(100),
    CONSTRAINT fk_admin FOREIGN KEY (admin_id) REFERENCES Admins(admin_id)
);

-- 公告表
CREATE TABLE Announcements (
    announcement_id VARCHAR2(10) PRIMARY KEY,
    admin_id VARCHAR2(10),
    title VARCHAR2(100) NOT NULL, -- 公告标题
    content CLOB,
    status VARCHAR2(20) CHECK(status IN ('未发布', '已发布', '已删除')), -- 状态
    publish_time TIMESTAMP, -- 发布时间
    last_modified_time TIMESTAMP, -- 最近修改时间
    CONSTRAINT fk_admin_announcement FOREIGN KEY (admin_id) REFERENCES Admins(admin_id)
);

-- 管理员表
CREATE TABLE Admins (
    admin_id VARCHAR2(10) PRIMARY KEY,
    admin_name VARCHAR2(100),
    phone_number VARCHAR2(15)
);

-- 用户通知表
CREATE TABLE UserNotifications (
    notification_id VARCHAR2(10) PRIMARY KEY,
    user_id VARCHAR2(10),
    notification_type VARCHAR2(50),
    title VARCHAR2(100), -- 通知标题
    content CLOB, -- 通知内容
    notification_time TIMESTAMP, -- 通知时间
    CONSTRAINT fk_user_notification FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- 管理员通知表
CREATE TABLE AdminNotifications (
    notification_id VARCHAR2(10) PRIMARY KEY,
    admin_id VARCHAR2(10),
    notification_type VARCHAR2(50),
    title VARCHAR2(100), -- 通知标题
    content CLOB, -- 通知内容
    notification_time TIMESTAMP, -- 通知时间
    CONSTRAINT fk_admin_notification FOREIGN KEY (admin_id) REFERENCES Admins(admin_id)
);

-- 场地设备关系表
CREATE TABLE VenueEquipments (
    equipment_id VARCHAR2(10),
    venue_id VARCHAR2(10),
    installation_time TIMESTAMP,
    PRIMARY KEY (equipment_id, venue_id),
    CONSTRAINT fk_venue_equipment FOREIGN KEY (venue_id) REFERENCES Venues(venue_id),
    CONSTRAINT fk_equipment_venue FOREIGN KEY (equipment_id) REFERENCES Equipments(equipment_id)
);

-- 场地公告关系表
CREATE TABLE VenueAnnouncements (
    venue_id VARCHAR2(10),
    announcement_id VARCHAR2(10),
    PRIMARY KEY (venue_id, announcement_id),
    CONSTRAINT fk_venue_announcement FOREIGN KEY (venue_id) REFERENCES Venues(venue_id),
    CONSTRAINT fk_announcement FOREIGN KEY (announcement_id) REFERENCES Announcements(announcement_id)
);

-- 用户团体关系表
CREATE TABLE UserGroupMembership (
    user_id VARCHAR2(10),
    group_id VARCHAR2(10),
    join_time TIMESTAMP,
    role VARCHAR2(50),
    PRIMARY KEY (user_id, group_id),
    CONSTRAINT fk_user_group FOREIGN KEY (user_id) REFERENCES Users(user_id),
    CONSTRAINT fk_group_user FOREIGN KEY (group_id) REFERENCES Groups(group_id)
);

-- 用户预约关系表
CREATE TABLE UserReservations (
    reservation_id VARCHAR2(10),
    user_id VARCHAR2(10),
    check_in_time TIMESTAMP,
    status VARCHAR2(50),
    PRIMARY KEY (reservation_id, user_id),
    CONSTRAINT fk_user_reservation FOREIGN KEY (reservation_id) REFERENCES Reservations(reservation_id),
    CONSTRAINT fk_user_reservation_user FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

-- 场地管理关系表
CREATE TABLE VenueManagement (
    venue_id VARCHAR2(10),
    admin_id VARCHAR2(10),
    role VARCHAR2(50),
    PRIMARY KEY (venue_id, admin_id),
    CONSTRAINT fk_venue_management FOREIGN KEY (venue_id) REFERENCES Venues(venue_id),
    CONSTRAINT fk_admin_management FOREIGN KEY (admin_id) REFERENCES Admins(admin_id)
);

-- 设备保养记录表
CREATE TABLE MaintenanceRecords (
    maintenance_record_id VARCHAR2(10) PRIMARY KEY,
    equipment_id VARCHAR2(10),
    start_time TIMESTAMP,
    end_time TIMESTAMP,
    description CLOB,
    CONSTRAINT fk_equipment FOREIGN KEY (equipment_id) REFERENCES Equipments(equipment_id)
);
