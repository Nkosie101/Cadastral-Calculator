using System;
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
//using CommunityToolkit.Mvvm.ComponentModel;

namespace Surveyors_Calculator.Model;

public partial class Formulae : ObservableObject
{
    //public int id { get; set; }
    public string name { get; set; }
    public string image { get; set; }


    public double ScaleFactor { get; set; }

    public double GetScaleFactor(string E1, string N1, string E2, string N2, string E3, string N3, string E4, string N4)
    {

        ScaleFactor = Math.Sqrt((Math.Pow((Convert.ToDouble(E1) - Convert.ToDouble(E2)), 2)
                                       + Math.Pow((Convert.ToDouble(N1) - Convert.ToDouble(N2)), 2))
                                       /
                                       (Math.Pow((Convert.ToDouble(E3) - Convert.ToDouble(E4)), 2)
                                       + Math.Pow((Convert.ToDouble(N3) - Convert.ToDouble(N4)), 2)));

        return ScaleFactor;
    }

    public double Rotation { get; set; }

    public double GetRotation(string E1, string N1, string E2, string N2, string E3, string N3, string E4, string N4)
    {
        Rotation = ((Math.Atan((Convert.ToDouble(E4) - Convert.ToDouble(E3)) / (Convert.ToDouble(N4) - Convert.ToDouble(N3))))
        -
        (Math.Atan((Convert.ToDouble(E2) - Convert.ToDouble(E1)) / (Convert.ToDouble(N2) - Convert.ToDouble(N1)))));

        return Rotation;
    }

    public double TranslationEasting { get; set; }
    public double TranslationNorthing { get; set; }

    public (double TranslationEasting, double TranslationNorthing) GetTranslation(double ScaleFactor, double Rotation, string E1, string N1, string E3, string N3)
    {
        double a = ScaleFactor * Math.Cos(Rotation);
        double b = ScaleFactor * Math.Sin(Rotation);

        TranslationEasting = Convert.ToDouble(E1) - (Convert.ToDouble(E3) * a - Convert.ToDouble(N3) * b);
        TranslationNorthing = Convert.ToDouble(N1) - (Convert.ToDouble(E3) * b + Convert.ToDouble(N3) * a);

        return (TranslationEasting, TranslationNorthing);

    }

    [ObservableProperty]
    private string easting = "0";
    [ObservableProperty]
    private string northing = "0";
    [ObservableProperty]
    private string elevation = "0";

    public (string Easting, string Northing) Transform(string E5, string N5)
    {


        double a = ScaleFactor * Math.Cos(Rotation);
        double b = ScaleFactor * Math.Sin(Rotation);

        /*var Coordinates(string E5, string N5)
        {

        }
        */
        var EastingToFormat = Convert.ToDouble(E5) * a - Convert.ToDouble(N5) * b + TranslationEasting;
        var NorthingToFormat = Convert.ToDouble(N5) * a + Convert.ToDouble(E5) * b + TranslationNorthing;

        Easting = $"{EastingToFormat:F3}";
        Northing = $"{NorthingToFormat:F3}";

        return (Easting, Northing);
    }

    //3D Calcs

    /*
        public Double Theta1 { get; set; }
        public Double Theta2 { get; set; }
        public Double Theta3 { get; set; }

        public Double R11 { get; set; }
        public Double R12 { get; set; }
        public Double R13 { get; set; }
        public Double R21 { get; set; }
        public Double R22 { get; set; }
        public Double R23 { get; set; }
        public Double R31 { get; set; }
        public Double R32 { get; set; }
        public Double R33 { get; set; }

        public (double R11, double R12, double R13, double R21, double R22, double R23, double R31, double R32, double R33) Get3DRotations(double Theta1, double Theta2, double Theta3)
        {
            R11 = Math.Cos(Theta2) * Math.Cos(Theta3);
            R12 = Math.Sin(Theta1) * Math.Sin(Theta2) * Math.Cos(Theta3) + Math.Cos(Theta1) * Math.Sin(Theta3);
            R13 = (Math.Cos(Theta1) * Math.Sin(Theta2) * Math.Cos(Theta3)) * -1 + Math.Sin(Theta1) * Math.Sin(Theta3);
            R21 = (Math.Cos(Theta2) * Math.Sin(Theta3)) * -1;
            R22 = (Math.Sin(Theta1) * Math.Sin(Theta2) * Math.Sin(Theta3)) * -1 + Math.Cos(Theta1) * Math.Cos(Theta3);
            R23 = Math.Cos(Theta1) * Math.Sin(Theta2) * Math.Sin(Theta3) + Math.Sin(Theta1) * Math.Cos(Theta3);
            R31 = Math.Sin(Theta2);
            R32 = (Math.Sin(Theta1) * Math.Cos(Theta2)) * -1;
            R33 = Math.Cos(Theta1) * Math.Cos(Theta2);

            return (R11, R12, R13, R21, R22, R23, R31, R32, R33);
        }
        public Double S { get; set; }

        public double Get3DGetScaleFactor(string E1, string N1, string Z1, string E2, string N2, string Z2, string E3, string N3, string Z3, string E4, string N4, string Z4)
        {

            S = Math.Sqrt((Math.Pow((Convert.ToDouble(E1) - Convert.ToDouble(E2)), 2)
                                           + Math.Pow((Convert.ToDouble(N1) - Convert.ToDouble(N2)), 2)
                                           + Math.Pow((Convert.ToDouble(Z1) - Convert.ToDouble(Z2)), 2))
                                           /
                                           (Math.Pow((Convert.ToDouble(E3) - Convert.ToDouble(E4)), 2)
                                           + Math.Pow((Convert.ToDouble(N3) - Convert.ToDouble(N4)), 2)
                                           + Math.Pow((Convert.ToDouble(Z3) - Convert.ToDouble(Z4)), 2)));

            return S;
        }

        public double Tx { get; set; }
        public double Ty { get; set; }
        public double Tz { get; set; }

        public (double Tx, double Ty, double Tz) Get3DTranslations(double S, double R11, double R12, double R13, double R21, double R22, double R23, double R31, double R32, double R33, string E1, string N1, string Z1, string E3, string N3, string Z3)
        {
            double a = ScaleFactor * Math.Cos(Rotation);
            double b = ScaleFactor * Math.Sin(Rotation);

            Tx = Convert.ToDouble(E1) - S * (Convert.ToDouble(E3) * R11 + Convert.ToDouble(N3) * R21 + Convert.ToDouble(N3) * R31);
            Tx = Convert.ToDouble(E1) - S * (Convert.ToDouble(E3) * R12 + Convert.ToDouble(N3) * R22 + Convert.ToDouble(N3) * R32);
            Tx = Convert.ToDouble(E1) - S * (Convert.ToDouble(E3) * R13 + Convert.ToDouble(N3) * R22 + Convert.ToDouble(N3) * R33);

            return (Tx, Ty, Tz);

        }


        public Double X = S * (R11 * x + R21 * y + R31 * z) + Tx;
        public Double Y = S * (R12 * x + R22 * y + R32 * z) + Ty;
        public Double Z = S * (R13 * x + R23 * y + R33 * z) + Tz;

        [ObservableProperty]
        private string X = "0";
        [ObservableProperty]
        private string Y = "0";
        [ObservableProperty]
        private string Z = "0";

        public (string X, string Y, string Z) Transform3D(string E5, string N5, string Z5)
        {
            var EastingToFormat = S * (R11 * Convert.ToDouble(E5) + R21 * Convert.ToDouble(N5) + R31 * Convert.ToDouble(Z5)) + Tx;
            var NorthingToFormat = S * (R12 * Convert.ToDouble(E5) + R22 * Convert.ToDouble(N5) + R32 * Convert.ToDouble(Z5)) + Ty;
            var ElevationToFormat = S * (R13 * Convert.ToDouble(E5) + R23 * Convert.ToDouble(N5) + R33 * Convert.ToDouble(Z5)) + Tz;



            X = $"{EastingToFormat:F3}";
            Y = $"{NorthingToFormat:F3}";
            Z = $"{ElevationToFormat:F3}";

            return (X, Y, Z);
        } 
        */

    //3D Calcs

    Matrix<double> ToX1 { get; set; }
    Matrix<double> ToX2;
    Matrix<double> ToX3;
    Matrix<double> Fromx1;
    Matrix<double> Fromx2;
    Matrix<double> Fromx3;


    public (Matrix<double> ToX1, Matrix<double> ToX2, Matrix<double> ToX3, Matrix<double> Fromx1, Matrix<double> Fromx2, Matrix<double> Fromx3) createInitialMatrices(string E1, string N1, string Z1, string E2, string N2, string Z2, string E3, string N3, string Z3, string E4, string N4, string Z4, string E5, string N5, string Z5, string E6, string N6, string Z6)
    {

        ToX1 = DenseMatrix.OfArray(new double[,]
        {
            {Convert.ToDouble(N1)},
            {Convert.ToDouble(E1)},
            {Convert.ToDouble(Z1)}
        });

        ToX2 = DenseMatrix.OfArray(new double[,]
        {
            {Convert.ToDouble(N2)},
            {Convert.ToDouble(E2)},
            {Convert.ToDouble(Z2)}
        });

        ToX3 = DenseMatrix.OfArray(new double[,]
        {
            {Convert.ToDouble(N3)},
            {Convert.ToDouble(E3)},
            {Convert.ToDouble(Z3)}
        });

        Fromx1 = DenseMatrix.OfArray(new double[,]
        {
            {Convert.ToDouble(N4)},
            {Convert.ToDouble(E4)},
            {Convert.ToDouble(Z4)}
        });

        Fromx2 = DenseMatrix.OfArray(new double[,]
        {
            {Convert.ToDouble(N5)},
            {Convert.ToDouble(E5)},
            {Convert.ToDouble(Z5)}
        });

        Fromx3 = DenseMatrix.OfArray(new double[,]
        {

            {Convert.ToDouble(N6)},
            {Convert.ToDouble(E6)},
            {Convert.ToDouble(Z6)}
        });

        return (ToX1, ToX2, ToX3, Fromx1, Fromx2, Fromx3);
    }

    Matrix<double> ToCentroid;
    Matrix<double> FromCentroid;

    public void CentroidMatrices()
    {
        ToCentroid = (ToX3 + ToX2 + ToX1) * 1 / 3;
        FromCentroid = (Fromx3 + Fromx2 + Fromx1) * 1 / 3;

    }

    Matrix<double> ToX1Bar;
    Matrix<double> ToX2Bar;
    Matrix<double> ToX3Bar;
    Matrix<double> Fromx1Bar;
    Matrix<double> Fromx2Bar;
    Matrix<double> Fromx3Bar;

    public void CentredCoordinates()
    {
        ToX1Bar = ToX1 - ToCentroid;
        ToX2Bar = ToX2 - ToCentroid;
        ToX3Bar = ToX3 - ToCentroid;
        Fromx1Bar = Fromx1 - FromCentroid;
        Fromx2Bar = Fromx2 - FromCentroid;
        Fromx3Bar = Fromx3 - FromCentroid;

    }

    Matrix<double> H;

    public void CrossCovarianceMatrix()
    {
        H = (Fromx1Bar * ToX1Bar.Transpose()) + (Fromx2Bar * ToX2Bar.Transpose()) + (Fromx3Bar * ToX3Bar.Transpose());
    }

    Matrix<double> R;

    public Matrix<double> SVD()
    {
        CentroidMatrices();
        CentredCoordinates();
        CrossCovarianceMatrix();

        var svd = H.Svd(true);
        R = svd.VT.Transpose() * svd.U.Transpose();

        return R;
    }

    public Double S { get; set; }

    public Double S3D()
    {
        var To = ToX1Bar.Transpose() * R.Multiply(Fromx1Bar) + ToX2Bar.Transpose() * R.Multiply(Fromx2Bar) + ToX3Bar.Transpose() * R.Multiply(Fromx3Bar);
        var From = Fromx1Bar.Transpose() * Fromx1Bar + Fromx2Bar.Transpose() * Fromx2Bar + Fromx3Bar.Transpose() * Fromx3Bar;

        var SumTo = To[0, 0]; //+ To[0, 1] + To[0, 2];
        var SumFrom = From[0, 0];// + From[1, 0] + From[2, 0];


        S = SumTo / SumFrom;

        return S;
    }

    Matrix<double> T;

    public Matrix<double> Translation()
    {

        T = ToCentroid - S * R * FromCentroid;

        return T;

    }

    public (string Easting, string Northing, string Elevation) Transform3D(string E7, string N7, string Z7)
    {
        var FromMatrix = DenseMatrix.OfArray(new double[,]
        {
            {Convert.ToDouble(N7)},
            {Convert.ToDouble(E7)},
            {Convert.ToDouble(Z7)}
        });

        var ToMatrix = R.Multiply(FromMatrix).Multiply(S).Add(T);

        //AppShell.Current.DisplayAlert("Error", $" {ToMatrix.ToString()}", "OK");

        Easting = $"{ToMatrix[0, 0]:F3}";
        Northing = $"{ToMatrix[1, 0]:F3}";
        Elevation = $"{ToMatrix[2, 0]:F3}";

        return (Easting, Northing, Elevation);
    }


    /*
     public double xBar
     { get; set; }
     public double yBar { get; set; }
     public double zBar { get; set; }
     public double ToXBar { get; set; }
     public double ToYBar { get; set; }
     public double ToZBar { get; set; }

     public (double xBar, double yBar, double zBar, double ToXBar, double ToYBar, double ToZBar) Centroids(string E1, string N1, string Z1, string E2, string N2, string Z2, string E3, string N3, string Z3, string E4, string N4, string Z4, string E5, string N5, string Z5, string E6, string N6, string Z6)
     {
         xBar = (Convert.ToDouble(E4) + Convert.ToDouble(E5) + Convert.ToDouble(E6)) / 3;
         yBar = (Convert.ToDouble(N4) + Convert.ToDouble(N5) + Convert.ToDouble(N6)) / 3;
         zBar = (Convert.ToDouble(Z4) + Convert.ToDouble(Z5) + Convert.ToDouble(Z6)) / 3;

         ToXBar = (Convert.ToDouble(E1) + Convert.ToDouble(E2) + Convert.ToDouble(E3)) / 3;
         ToYBar = (Convert.ToDouble(N1) + Convert.ToDouble(N2) + Convert.ToDouble(N3)) / 3;
         ToZBar = (Convert.ToDouble(Z1) + Convert.ToDouble(Z2) + Convert.ToDouble(Z3)) / 3;

         return (xBar, yBar, zBar, ToXBar, ToYBar, ToZBar);
     }

     public (string E1, string N1, string Z1, string E2, string N2, string Z2, string E3, string N3, string Z3, string E4, string N4, string Z4, string E5, string N5, string Z5, string E6, string N6, string Z6) CentrePoints(string E1, string N1, string Z1, string E2, string N2, string Z2, string E3, string N3, string Z3, string E4, string N4, string Z4, string E5, string N5, string Z5, string E6, string N6, string Z6)
     {

     }

     void createMatrix()
     {
         A = DenseMatrix.OfArray(new double[,]
                 {
                     {A00,A01,A02},
                     {A10,A11,A12},
                     {A20,A21,A22}
                 });
         // E1Entry = A[0, 0].ToString();
         //A00 = A[0, 0].ToString();
         //A01 = A[0, 1].ToString();
         //A10 = A[1, 0].ToString();
         //A11 = A[1, 1].ToString();*


     }
     Matrix<double> A;

     [ObservableProperty]
     private string e1Entry;

     [ObservableProperty]
     private double a00;
     [ObservableProperty]
     private double a01;
     [ObservableProperty]
     private double a02;
     [ObservableProperty]
     private double a10;
     [ObservableProperty]
     private double a11;
     [ObservableProperty]
     private double a12;
     [ObservableProperty]
     private double a20;
     [ObservableProperty]
     private double a21;
     [ObservableProperty]
     private double a22;

     [ObservableProperty]
     private double b00;
     [ObservableProperty]
     private double b01;
     [ObservableProperty]
     private double b02;
     [ObservableProperty]
     private double b10;
     [ObservableProperty]
     private double b11;
     [ObservableProperty]
     private double b12;
     [ObservableProperty]
     private double b20;
     [ObservableProperty]
     private double b21;
     [ObservableProperty]
     private double b22;




     [RelayCommand]
     async Task GetTransformedCoordinates()
     {
         if (IsBusy)
             return;


         try
         {
             IsBusy = true;
             //var scaleFactor = formulae.GetScaleFactor(e1Entry, n1Entry, e2Entry, n2Entry, e3Entry, n3Entry, e4Entry, n4Entry);
             //Easting = scaleFactor.ToString();
             // var rotation = formulae.GetRotation(e1Entry, n1Entry, e2Entry, n2Entry, e3Entry, n3Entry, e4Entry, n4Entry);
             //formulae.GetTranslation(scaleFactor, rotation, e1Entry, n1Entry, e3Entry, n3Entry);

             //var result = formulae.Transform(e5Entry, n5Entry);
             //  Easting = result.Easting.ToString();
             // Northing = result.Northing.ToString();
             //await AppShell.Current.DisplayAlert("Result", $"E: {result.Easting}, N: {result.Northing}", "OK");
             //await AppShell.Current.DisplayAlert("Error!", "11", "OK");createMat();
             createMat();
             Matrix<double> B = A.Transpose();
             B00 = B[0, 0];
             B01 = B[0, 1];
             B02 = B[0, 2];
             B10 = B[1, 0];
             B11 = B[1, 1];
             B12 = B[1, 2];
             B20 = B[2, 0];
             B21 = B[2, 1];
             B22 = B[2, 2];
         }
         catch (Exception e)
         {
             await AppShell.Current.DisplayAlert("Error", $"Check your entries! {e.Message}", "OK");
         }
         finally
         {
             IsBusy = false;
         }
     }*/

}

