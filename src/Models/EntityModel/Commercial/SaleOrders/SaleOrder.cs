/*
 * Copyright (c) gestaoaju.com.br - All rights reserved.
 * Licensed under MIT (https://github.com/gestaoaju/commerce/blob/master/LICENSE).
 */

using Gestaoaju.Infrastructure.Tenancy;
using Gestaoaju.Models.EntityModel.Financial;
using Gestaoaju.Models.EntityModel.Inventory;
using Gestaoaju.Models.EntityModel.Commercial.Customers;
using Gestaoaju.Models.EntityModel.Account.Tenants;
using Gestaoaju.Models.EntityModel.Manage.Stores;
using Gestaoaju.Models.EntityModel.Financial.SalePayments;
using Gestaoaju.Models.EntityModel.Commercial.SaleProducts;
using Gestaoaju.Models.EntityModel.Commercial.SaleServices;
using Gestaoaju.Models.EntityModel.Financial.Wallets;
using System;
using System.Collections.Generic;

namespace Gestaoaju.Models.EntityModel.Commercial.SaleOrders
{
    public class SaleOrder : ITenantScope, IInvoice, IOutgoingOrder
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int StoreId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public int? CustomerId { get; set; }
        public int? WalletId { get; set; }
        public decimal Total { get; set; }
        public decimal? Discount { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal BilledAmount { get; set; }
        public string Author { get; set; }
        public string AdditionalInformation { get; set; }
        public bool Confirmed => ConfirmationDate != null;
        public bool Budget => !Confirmed && Total > 0 && new Money(Total)
            .SubtractPercentage(Discount ?? 0) == TotalPayable;
        public bool Draft => !Confirmed && (Total == 0 || new Money(Total)
            .SubtractPercentage(Discount ?? 0) != TotalPayable);
        public Customer Customer { get; set; }
        public Tenant Tenant { get; set; }
        public Store Store { get; set; }
        public Wallet Wallet { get; set; }
        public ICollection<SalePayment> SalePayments { get; set; }
        public ICollection<SaleProduct> SaleProducts { get; set; }
        public ICollection<SaleService> SaleServices { get; set; }
        DateTime IOutgoingOrder.Date => IssueDate;
        IEnumerable<IOutgoingProduct> IOutgoingOrder.OutgoingList => SaleProducts;
        decimal IInvoice.Total
        {
            get { return BilledAmount; }
            set { BilledAmount = value; }
        }
        IEnumerable<IPayment> IInvoice.Payments => SalePayments;
    }
}
