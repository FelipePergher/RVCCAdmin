// <copyright file="ISaleShirt2020.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System;

namespace RVCC.Data.Interfaces
{
    public interface ISaleShirt2020
    {
        public int ShirtSaleId { get; set; }

        public int ShirtQuantityTotal { get; set; }

        public double PriceTotal { get; set; }

        public string BuyerName { get; set; }

        public string BuyerPhone { get; set; }

        #region Regions

        public DateTime DateOrdered { get; set; }

        public DateTime DatePayment { get; set; }

        public DateTime DateConfection { get; set; }

        public DateTime DateProduced { get; set; }

        public DateTime DateCollected { get; set; }

        public DateTime DateCanceled { get; set; }

        public Enums.Status Status { get; set; }

        #endregion

        // 5 reais
        public int MaskQuantity { get; set; }

        // R$ 20
        #region Shirt Younger Sizes

        public int Size2NormalQuantity { get; set; }

        public int Size4NormalQuantity { get; set; }

        public int Size6NormalQuantity { get; set; }

        public int Size8NormalQuantity { get; set; }

        public int Size10NormalQuantity { get; set; }

        public int Size12NormalQuantity { get; set; }

        public int Size14NormalQuantity { get; set; }

        public int Size16NormalQuantity { get; set; }

        #endregion

        #region Shirt Normal Sizes

        public int SizePNormalQuantity { get; set; }

        public int SizeMNormalQuantity { get; set; }

        public int SizeGNormalQuantity { get; set; }

        public int SizeGGNormalQuantity { get; set; }

        public int SizeXGNormalQuantity { get; set; }

        #endregion

        #region Shirt Baby Look Sizes

        public int SizePBabyLookQuantity { get; set; }

        public int SizeMBabyLookQuantity { get; set; }

        public int SizeGBabyLookQuantity { get; set; }

        public int SizeGGBabyLookQuantity { get; set; }

        public int SizeXGBabyLookQuantity { get; set; }

        #endregion
    }
}