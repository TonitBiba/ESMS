﻿using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class AspNetUsers
    {
        public AspNetUsers()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaims>();
            AspNetUserLogins = new HashSet<AspNetUserLogins>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetUserTokens = new HashSet<AspNetUserTokens>();
            EmployeeDocuments = new HashSet<EmployeeDocuments>();
            Leaves = new HashSet<Leaves>();
            LeavesDetails = new HashSet<LeavesDetails>();
            Menu = new HashSet<Menu>();
            NotificationsVcInsertedUserNavigation = new HashSet<Notifications>();
            NotificationsVcUserNavigation = new HashSet<Notifications>();
            PaymentsUser = new HashSet<Payments>();
            PaymentsVcInsertedNavigation = new HashSet<Payments>();
            SubMenu = new HashSet<SubMenu>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public int? City { get; set; }
        public int? Country { get; set; }
        public int EmployeeStatus { get; set; }
        public int Gender { get; set; }
        public string JobTitle { get; set; }
        public int? PostCode { get; set; }
        public string IbanCode { get; set; }
        public string PersonalNumber { get; set; }
        public float Salary { get; set; }
        public byte[] UserProfile { get; set; }
        public DateTime DtFrom { get; set; }
        public DateTime DtTo { get; set; }
        public int TaxGroupId { get; set; }
        public int? Language { get; set; }
        public bool? ChangePassword { get; set; }

        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual ICollection<EmployeeDocuments> EmployeeDocuments { get; set; }
        public virtual ICollection<Leaves> Leaves { get; set; }
        public virtual ICollection<LeavesDetails> LeavesDetails { get; set; }
        public virtual ICollection<Menu> Menu { get; set; }
        public virtual ICollection<Notifications> NotificationsVcInsertedUserNavigation { get; set; }
        public virtual ICollection<Notifications> NotificationsVcUserNavigation { get; set; }
        public virtual ICollection<Payments> PaymentsUser { get; set; }
        public virtual ICollection<Payments> PaymentsVcInsertedNavigation { get; set; }
        public virtual ICollection<SubMenu> SubMenu { get; set; }
    }
}
