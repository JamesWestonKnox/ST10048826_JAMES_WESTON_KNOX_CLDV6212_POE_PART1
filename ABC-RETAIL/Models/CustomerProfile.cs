﻿using Azure;
using Azure.Data.Tables;
using System;

namespace ABC_RETAIL.Models
{
    public class CustomerProfile : ITableEntity
    {
        //getters and setters to create customer profiles for the table storage
        //unique partitin key used in table storage
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public CustomerProfile()
        {
            PartitionKey = "CustomerProfile";
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
//McCall, B., 2024. CLDV_SemesterTwo_Byron. [online] GitHub.Available at: https://github.com/ByronMcCallLecturer/CLDV_SemesterTwo_Byron [Accessed 29 August 2024].