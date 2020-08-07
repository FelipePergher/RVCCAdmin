// <copyright file="SaleShirt2020FormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class SaleShirt2020FormModel
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string BuyerName { get; set; }

        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Estado")]
        public string Status { get; set; }

        [Display(Name = "Telefone")]
        public string BuyerPhone { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Display(Name = "Data do pedido")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string DateOrdered { get; set; }

        [Display(Name = "Data do depósito")]
        public string DatePayment { get; set; }

        [Display(Name = "Enviado para confecção")]
        public string DateConfection { get; set; }

        [Display(Name = "Produzido em")]
        public string DateProduced { get; set; }

        [Display(Name = "Retirado em")]
        public string DateCollected { get; set; }

        [Display(Name = "Cancelado em")]
        public string DateCanceled { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "Máscaras")]
        public int MaskQuantity { get; set; }

        public double PriceTotal { get; set; }

        public int ShirtQuantityTotal { get; set; }

        #region Shirt Normal Sizes

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "4")]
        public int Size4NormalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "8")]
        public int Size8NormalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "12")]
        public int Size12NormalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "14")]
        public int Size14NormalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "16")]
        public int Size16NormalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "P")]
        public int SizePNormalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "M")]
        public int SizeMNormalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "G")]
        public int SizeGNormalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "GG")]
        public int SizeGGNormalQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "XG")]
        public int SizeXGNormalQuantity { get; set; }

        #endregion

        #region Shirt Baby Look Sizes

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "P")]
        public int SizePBabyLookQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "M")]
        public int SizeMBabyLookQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "G")]
        public int SizeGBabyLookQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "GG")]
        public int SizeGGBabyLookQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa")]
        [Display(Name = "XG")]
        public int SizeXGBabyLookQuantity { get; set; }

        #endregion
    }
}
