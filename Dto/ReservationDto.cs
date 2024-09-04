namespace VenueBookingSystem.Dto
{
    public class ReservationDto
    {
        public decimal PaymentAmount { get; set; }  
        public string UserId { get; set; }  
        public string VenueId { get; set; }  
        public string AvailabilityId { get; set; }  
        public string ReservationItem { get; set; }  
        
        public required string ReservationType { get; set; }  
    }
    public class ReservationDetailDto
    {
        public  string UserId { get; set; } // �û�ID
        public DateTime? CheckInTime { get; set; } // �û�ǩ��ʱ��
        public  string Status { get; set; } // ԤԼ״̬
        public string Username { get; set; } // �û���

        public string RealName { get; set; }  // ��ʵ����
    }

    public class UpdateReservationUserDto
    {
        
        public string ReservationId { get; set; } // �û�ID
        public string UserId { get; set; } // �û�ID
        public DateTime? CheckInTime { get; set; } // �û�ǩ��ʱ��
        public string Status { get; set; } // ԤԼ״̬
    }

    public class ReservationListDto
    {
        public  string ReservationId { get; set; } // ԤԼID
        public  string VenueId { get; set; } // ����ID
        public  string AvailabilityId { get; set; } // ����ʱ���ID
        public  string ReservationType { get; set; }  //ԤԼ���ͣ��û������壩

        public  DateTime ReservationTime { get; set; } // ԤԼ����ʱ��
        public  decimal PaymentAmount { get; set; } // ֧�����

        public int NumOfPeople { get; set; }

        public string VenueName { get; set; } // ��������
        public  DateTime StartTime { get; set; } // ��ʼʱ��
        public  DateTime EndTime { get; set; } // ����ʱ��
        public List<GroupReservationListDto> GroupReservationListDto { get; set; }
    }
    public class GroupReservationListDto
    {
        public  string GroupId { get; set; } // ����ID

        public  string GroupName { get; set; } // ��������
    }



    public class ReservationVenueListDto
    {
        public string ReservationId { get; set; } // ԤԼID
        public string VenueId { get; set; } // ����ID
        public string AvailabilityId { get; set; } // ����ʱ���ID
        public string ReservationType { get; set; }  //ԤԼ���ͣ��û������壩

        public DateTime ReservationTime { get; set; } // ԤԼ����ʱ��
        public decimal PaymentAmount { get; set; } // ֧�����

        public int NumOfPeople { get; set; }

        public string VenueName { get; set; } // ��������
        public DateTime StartTime { get; set; } // ��ʼʱ��
        public DateTime EndTime { get; set; } // ����ʱ��
       
    }

}
