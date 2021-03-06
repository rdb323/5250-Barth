﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Mine.Models;
using Mine.ViewModels;

namespace Mine.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemUpdatePage : ContentPage
    {
        void Value_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            ValueValue.Text = String.Format("{0}", e.NewValue);
        }

        public ItemModel Item { get; set; }

        public ItemUpdatePage()
        {
            InitializeComponent();

            Item = new ItemModel
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        /// <summary>
        /// constructor that takes a ViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        public ItemUpdatePage(ItemReadViewModel viewModel)
        {
            InitializeComponent();
            Item = viewModel.Item;

            BindingContext = this;
        }

        async void UpdateItem(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "UpdateItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}