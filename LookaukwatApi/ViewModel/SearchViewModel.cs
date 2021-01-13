using LookaukwatApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LookaukwatApi.ViewModel
{
    public class SearchViewModel
    {
        public int id { get; set; }
        public int ViewNumber { get; set; }
        //For all product
        public string Title { get; set; }
        public DateTime DateAdd { get; set; }
        public string Category { get; set; }
        public string Town { get; set; }
        public string SearchOrAskJob { get; set; }
        public int Price { get; set; }
       public  List<ImageProcductModel> Images { get; set; }
        //For Job
        public string TypeContract { get; set; }
        public string ActivitySector { get; set; }

        //For Mode

        public string RubriqueMode { get; set; }
        public string TypeMode { get; set; }
        public string BrandMode { get; set; }
        public string UniversMode { get; set; }
        public string SizeMode { get; set; }
        public string State { get; set; }
        public string ColorMode { get; set; }

        //House

        public string RubriqueHouse { get; set; }
        public string TypeHouse { get; set; }
        public string FabricMaterialHouse { get; set; }
        public string StateHouse { get; set; }
        public string ColorHouse { get; set; }

        //Multimedia

        public string MultimediaRubrique;
        public string MultimediaBrand;
        public string MultimediaModel;
        public string MultimediaCapacity;

        //Apart

        public int RoomNumberAppart { get; set; }
        public string FurnitureOrNotAppart { get; set; }
        public string TypeAppart { get; set; }
        public int ApartSurfaceAppart { get; set; }

        //Vehicule

        public string VehiculeRubrique { get; set; }
        public string VehiculeBrand { get; set; }
        public string VehiculeModel { get; set; }
        public string VehiculeType { get; set; }
        public string Petrol { get; set; }
        public string Year { get; set; }
        public string Mileage { get; set; }
        public string NumberOfDoor { get; set; }
        public string GearBox { get; set; }
        public string Vehiculestate { get; set; }
        public string Color { get; set; }
    }
}