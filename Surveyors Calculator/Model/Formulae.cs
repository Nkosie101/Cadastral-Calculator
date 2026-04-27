using System;
using System.Linq;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
//using CommunityToolkit.Mvvm.ComponentModel;
using Android.Opengl;

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

    Matrix<double> ToX { get; set; }
    Matrix<double> FromX { get; set; }
    Matrix<double> ToTransformX { get; set; }

    //public List<Matrix> ToCoords = new List<Matrix>();
    //public List<Matrix> FromCoords = new List<Matrix>();


    public (List<Matrix<double>> ToCoords, List<Matrix<double>> FromCoords, List<Matrix<double>> ToTransform) createInitialMatrices(List<Coordinate> To, List<Coordinate> From, List<Coordinate> Transform)
    {
        ToCoords.Clear();
        FromCoords.Clear();
        ToTransform.Clear();
        foreach (Coordinate coordinate in To)
        {
            ToX = DenseMatrix.OfArray(new double[,]
        {
            {coordinate.x},
            {coordinate.y},
            {coordinate.z}
        });
            ToCoords.Add(ToX);
        }

        foreach (Coordinate coordinate in From)
        {
            FromX = DenseMatrix.OfArray(new double[,]
        {
            {coordinate.x},
            {coordinate.y},
            {coordinate.z}
        });
            FromCoords.Add(FromX);
        }

        foreach (Coordinate coordinate in Transform)
        {
            ToTransformX = DenseMatrix.OfArray(new double[,]
        {
            {coordinate.x},
            {coordinate.y},
            {coordinate.z}
        });
            //AppShell.Current.DisplayAlert("Error", $"ToTransform behind {ToTransform.Count}", "OK");
            ToTransform.Add(ToTransformX);
        }

        count = ToCoords.Count;
        /*AppShell.Current.DisplayAlert("Error", $"ToCoords {ToCoords.Count}", "OK");
        AppShell.Current.DisplayAlert("Error", $"FromCoords {FromCoords.Count}", "OK");
        AppShell.Current.DisplayAlert("Error", $"ToTransform {ToTransform.Count}", "OK");*/

        return (ToCoords, FromCoords, ToTransform);
    }

    //Matrix<double> ToCentroid;
    //Matrix<double> FromCentroid;
    Matrix<double> sumTo;
    Matrix<double> sumFrom;

    public void CentroidMatricesIDW()
    {
        sumTo = ToCoords.Aggregate((a, b) => a + b);
        sumFrom = FromCoords.Aggregate((a, b) => a + b);
        ToCentroid = (sumTo) * 1 / ToCoords.Count;
        FromCentroid = (sumFrom) * 1 / FromCoords.Count;
        //AppShell.Current.DisplayAlert("Error", $"{ToCentroid}", "OK");
        //AppShell.Current.DisplayAlert("Error", $"{FromCentroid}", "OK");
    }

    public List<Matrix<double>> ToCoordsBar = new List<Matrix<double>>();
    public List<Matrix<double>> FromCoordsBar = new List<Matrix<double>>();

    public void CenteredCoordinatesIDW()
    {
        foreach (Matrix<double> uncenteredFrom in FromCoords)
        {
            var centeredFrom = uncenteredFrom - FromCentroid;
            FromCoordsBar.Add(centeredFrom);
            //AppShell.Current.DisplayAlert("Res", $"delta {centeredFrom}", "OK");
        }

        foreach (Matrix<double> uncenteredTo in ToCoords)
        {
            var centeredTo = uncenteredTo - ToCentroid;
            ToCoordsBar.Add(centeredTo);
            //AppShell.Current.DisplayAlert("Res", $"delta {centeredTo}", "OK");
        }
    }

    Matrix<double> ToBar;
    Matrix<double> FromBar;

    double KappaNot = 0;
    Matrix<double> Initial;

    public Matrix<double> InitialApproximations()
    {
        //AppShell.Current.DisplayAlert("Error", $"{ToCoords.Count}", "OK");
        double angleFrom = 0;
        double angleTo = 0;

        double length = 0;
        var id1 = 0;
        var id2 = 0;

        for (int i = 0; i < FromCoords.Count; i++)
        {
            for (int j = 0; j < FromCoords.Count; j++)
            {
                //AppShell.Current.DisplayAlert("Error", $"{length}", "OK");
                var lengthToCompare = (FromCoords[i] - FromCoords[j]).FrobeniusNorm();
                //AppShell.Current.DisplayAlert("Error", $"{lengthToCompare}", "OK");
                if (lengthToCompare > length)
                {
                    length = lengthToCompare;
                    id1 = i;
                    id2 = j;

                }
            }
        }
        //AppShell.Current.DisplayAlert("Error", $"{FromCoords[id1]} {FromCoords[id2]}", "OK");
        foreach (Matrix<double> coord in FromCoords)
        {

        }

        angleFrom = Math.Atan2((FromCoords[id2][1, 0] - FromCoords[id1][1, 0]), (FromCoords[id2][0, 0] - FromCoords[id1][0, 0]));
        angleTo = Math.Atan2((ToCoords[id2][1, 0] - ToCoords[id1][1, 0]), (ToCoords[id2][0, 0] - ToCoords[id1][0, 0]));

        KappaNot = angleTo - angleFrom;
        while (KappaNot < 0)
        {
            KappaNot += 2 * Math.PI;
        }
        //AppShell.Current.DisplayAlert("Error", $"{angleFrom}", "OK");
        //AppShell.Current.DisplayAlert("Error", $"{angleTo}", "OK");
        //AppShell.Current.DisplayAlert("Error", $"{KappaNot}", "OK");

        var ScaleNot = Math.Sqrt((Math.Pow((ToCoords[id2][0, 0] - ToCoords[id1][0, 0]), 2) + Math.Pow((ToCoords[id2][1, 0] - ToCoords[id1][1, 0]), 2) + Math.Pow((ToCoords[id2][2, 0] - ToCoords[id1][2, 0]), 2)) / (Math.Pow((FromCoords[id2][0, 0] - FromCoords[id1][0, 0]), 2) + Math.Pow((FromCoords[id2][1, 0] - FromCoords[id1][1, 0]), 2) + Math.Pow((FromCoords[id2][2, 0] - FromCoords[id1][2, 0]), 2)));

        //AppShell.Current.DisplayAlert("Error", $"{ScaleNot}", "OK");
        Matrix<double> sumedFrom = Matrix<double>.Build.Dense(3, 1);
        Matrix<double> sumedTo = Matrix<double>.Build.Dense(3, 1);

        /*foreach (Matrix<double> coord in FromCoords)
        {
            //CentroidMatricesIDW();
            //AppShell.Current.DisplayAlert("Error", $"{id2}", "OK");
            sumFrom += coord;
            AppShell.Current.DisplayAlert("Error", $"{sumFrom}", "OK");
        }

        FromBar = sumedFrom * 1 / FromCoords.Count;

        foreach (Matrix<double> coord in ToCoords)
        {
            sumTo += coord;
        }*/

        CentroidMatricesIDW();
        CenteredCoordinatesIDW();

        //ToBar = sumedTo * 1 / ToCoords.Count;
        //AppShell.Current.DisplayAlert("Error", $"{FromBar} {ToBar}", "OK");

        var x1Bar = FromCentroid[0, 0] * Math.Cos(KappaNot) - FromCentroid[1, 0] * Math.Sin(KappaNot);
        var y1Bar = FromCentroid[0, 0] * Math.Sin(KappaNot) + FromCentroid[1, 0] * Math.Cos(KappaNot);
        var z1Bar = FromCentroid[2, 0];

        var TNotX = ToCentroid[0, 0] - ScaleNot * x1Bar;
        var TNotY = ToCentroid[1, 0] - ScaleNot * y1Bar;
        var TNotZ = ToCentroid[2, 0] - ScaleNot * z1Bar;

        Matrix<double> TranslationNot = DenseMatrix.OfArray(new double[,]
                {
                {TNotX},
                {TNotY},
                {TNotZ}
                });


        Double OmegaNot = 0;
        Double PhiNot = 0;

        Double R11 = 0;
        Double R12 = 0;
        Double R13 = 0;
        Double R21 = 0;
        Double R22 = 0;
        Double R23 = 0;
        Double R31 = 0;
        Double R32 = 0;
        Double R33 = 0;


        R11 = Math.Cos(PhiNot) * Math.Cos(KappaNot);
        R12 = Math.Sin(OmegaNot) * Math.Sin(PhiNot) * Math.Cos(KappaNot) + Math.Cos(OmegaNot) * Math.Sin(KappaNot);
        R13 = (Math.Cos(OmegaNot) * Math.Sin(PhiNot) * Math.Cos(KappaNot)) * -1 + Math.Sin(OmegaNot) * Math.Sin(KappaNot);
        R21 = (Math.Cos(PhiNot) * Math.Sin(KappaNot)) * -1;
        R22 = (Math.Sin(OmegaNot) * Math.Sin(PhiNot) * Math.Sin(KappaNot)) * -1 + Math.Cos(OmegaNot) * Math.Cos(KappaNot);
        R23 = Math.Cos(OmegaNot) * Math.Sin(PhiNot) * Math.Sin(KappaNot) + Math.Sin(OmegaNot) * Math.Cos(KappaNot);
        R31 = Math.Sin(PhiNot);
        R32 = (Math.Sin(OmegaNot) * Math.Cos(PhiNot)) * -1;
        R33 = Math.Cos(OmegaNot) * Math.Cos(PhiNot);

        Matrix<double> RNot = DenseMatrix.OfArray(new double[,]
                {
                {R11,R12,R13},
                {R21,R22,R23},
                {R31,R32,R33}
                });


        T = TranslationNot;
        R = RNot;
        S = ScaleNot;

        CalcPoints();

        Initial = DenseMatrix.OfArray(new double[,]
        {
            {ScaleNot},
            {OmegaNot},
            {PhiNot},
            {KappaNot},
            {TNotX},
            {TNotY},
            {TNotZ}
        });
        //AppShell.Current.DisplayAlert("Error", $"{Initial}", "OK");
        Omega = OmegaNot;
        Phi = PhiNot;
        Kappa = KappaNot;

        ParameterVector = Initial;

        //AppShell.Current.DisplayAlert("Res", $"o {Omega} p {Phi} k{Kappa} t{T} s{S} r{R}", "OK");

        return Initial;
    }

    public void GetParameters()
    {
        //AppShell.Current.DisplayAlert("Res", $"o {Omega} p {Phi} k{Kappa}", "OK");
        Get3DRotationsIDW(Omega, Phi, Kappa);
        DesignMatrix();
        CalcPoints();
    }

    public Double S { get; set; }

    public Double S3DIDW()
    {
        var To = ToCentroid.Transpose() * R.Multiply(FromCentroid) + ToCentroid.Transpose() * R.Multiply(FromCentroid) + ToCentroid.Transpose() * R.Multiply(FromCentroid);
        var From = FromCentroid.Transpose() * FromCentroid + FromCentroid.Transpose() * FromCentroid + FromCentroid.Transpose() * FromCentroid;

        var SumTo = To[0, 0]; //+ To[0, 1] + To[0, 2];
        var SumFrom = From[0, 0];// + From[1, 0] + From[2, 0];


        S = SumTo / SumFrom;

        return S;
    }

    int count = 0;
    Matrix<double> W;
    int iteration = 1;
    Matrix<double> target;

    public Matrix<double> Weights(Matrix<double> targetPoint)
    {
        target = targetPoint;
        W = Matrix<double>.Build.Dense(ToCoords.Count * 3, ToCoords.Count * 3);
        double distance = 0;
        /*for (int i = 0; i < ToTransform.Count; i++)
        {
            if (iteration == i)
        }*/

        for (int j = 0; j < FromCoords.Count; j++)
        {
            var pos = j * 3;
            var diff = FromCoords[j] - FromCentroid;
            distance = Math.Pow(diff.FrobeniusNorm(), 2); //AppShell.Current.DisplayAlert("Error", $"{diff}", "OK");
            W[pos, pos] = 1 / distance;
            W[pos + 1, pos + 1] = 1 / distance;
            W[pos + 2, pos + 2] = 1 / distance;
        }
        //AppShell.Current.DisplayAlert("Error", $"{W}", "OK");
        return W;
    }

    public Double Omega { get; set; }
    public Double Phi { get; set; }
    public Double Kappa { get; set; }

    public Double R11 { get; set; }
    public Double R12 { get; set; }
    public Double R13 { get; set; }
    public Double R21 { get; set; }
    public Double R22 { get; set; }
    public Double R23 { get; set; }
    public Double R31 { get; set; }
    public Double R32 { get; set; }
    public Double R33 { get; set; }

    Matrix<double> R = Matrix<double>.Build.Dense(3, 3);

    public Matrix<double> Get3DRotationsIDW(double Omega, double Phi, double Kappa)
    {
        /*AppShell.Current.DisplayAlert("Error", $"{Omega}", "OK");
        AppShell.Current.DisplayAlert("Error", $"{Phi}", "OK");
        AppShell.Current.DisplayAlert("Error", $"{Kappa}", "OK");*/
        R11 = Math.Cos(Phi) * Math.Cos(Kappa);
        R12 = Math.Sin(Omega) * Math.Sin(Phi) * Math.Cos(Kappa) + Math.Cos(Omega) * Math.Sin(Kappa);
        R13 = (Math.Cos(Omega) * Math.Sin(Phi) * Math.Cos(Kappa)) * -1 + Math.Sin(Omega) * Math.Sin(Kappa);
        R21 = (Math.Cos(Phi) * Math.Sin(Kappa)) * -1;
        R22 = (Math.Sin(Omega) * Math.Sin(Phi) * Math.Sin(Kappa)) * -1 + Math.Cos(Omega) * Math.Cos(Kappa);
        R23 = Math.Cos(Omega) * Math.Sin(Phi) * Math.Sin(Kappa) + Math.Sin(Omega) * Math.Cos(Kappa);
        R31 = Math.Sin(Phi);
        R32 = (Math.Sin(Omega) * Math.Cos(Phi)) * -1;
        R33 = Math.Cos(Omega) * Math.Cos(Phi);



        R[0, 0] = R11;
        R[0, 1] = R12;
        R[0, 2] = R13;
        R[1, 0] = R21;
        R[1, 1] = R22;
        R[1, 2] = R23;
        R[2, 0] = R31;
        R[2, 1] = R32;
        R[2, 2] = R33;
        //AppShell.Current.DisplayAlert("Error", $"{R}", "OK");
        return R;

    }

    Matrix<double> A;
    public Matrix<double> DesignMatrix()
    {
        //var x1 = target[0, 0] * Math.Cos(Kappa) + target[1, 0] * Math.Sin(Kappa);
        //var y1 = FromCoords[i][0, 0] * Math.Sin(Kappa) + FromCoords[i][1, 0] * Math.Cos(Kappa);
        //var z1 = FromCoords[i][2, 0];
        var rows = FromCoords.Count * 3; //AppShell.Current.DisplayAlert("Error", $"{x1}", "OK");

        A = Matrix<double>.Build.Dense(rows, 7);

        for (int i = 0; i < FromCoords.Count; i++)
        {
            var x1 = FromCoords[i][0, 0] * Math.Cos(Kappa) - FromCoords[i][1, 0] * Math.Sin(Kappa);
            var y1 = FromCoords[i][0, 0] * Math.Sin(Kappa) + FromCoords[i][1, 0] * Math.Cos(Kappa);
            var z1 = FromCoords[i][2, 0];
            var x = FromCoords[i][0, 0];
            var y = FromCoords[i][1, 0];
            var z = FromCoords[i][2, 0];

            //AppShell.Current.DisplayAlert("Res", $"cen {x} {y} {z}", "OK");

            /*var x1 = FromCoordsBar[i][0, 0] * Math.Cos(Kappa) - FromCoordsBar[i][1, 0] * Math.Sin(Kappa);
            var y1 = FromCoordsBar[i][0, 0] * Math.Sin(Kappa) + FromCoordsBar[i][1, 0] * Math.Cos(Kappa);
            var z1 = FromCoordsBar[i][2, 0];*/
            //var x1 = FromCoords[i][0, 0] * R11 + FromCoords[i][1, 0] * R21 + FromCoords[i][2, 0] * R31;
            //var y1 = FromCoords[i][0, 0] * R12 + FromCoords[i][1, 0] * R22 + FromCoords[i][2, 0] * R32;
            //var z1 = FromCoords[i][0, 0] * R13 + FromCoords[i][1, 0] * R23 + FromCoords[i][2, 0] * R33;
            var pos = i * 3;

            double delXScale = R11 * x + R21 * y + R31 * z;
            double delYScale = R12 * x + R22 * y + R32 * z;
            double delZScale = R13 * x + R23 * y + R33 * z;
            double delYOmega = -S * (R13 * x + R23 * y + R33 * z);
            double delZOmega = S * (R12 * x + R22 * y + R32 * z);
            double delXPhi = S * (-x * Math.Sin(Phi) * Math.Cos(Kappa) + y * Math.Sin(Phi) * Math.Sin(Kappa) + z * Math.Cos(Phi));
            double delYPhi = S * (x * Math.Sin(Omega) * Math.Cos(Phi) * Math.Cos(Kappa) - y * Math.Sin(Omega) * Math.Cos(Phi) * Math.Sin(Kappa) + z * Math.Sin(Omega) * Math.Sin(Phi));
            double delZPhi = S * (-x * Math.Cos(Omega) * Math.Cos(Phi) * Math.Cos(Kappa) + y * Math.Cos(Omega) * Math.Cos(Phi) * Math.Sin(Kappa) - z * Math.Cos(Omega) * Math.Sin(Phi));
            double delXKappa = S * (R21 * x - R11 * y);
            double delYKappa = S * (R22 * x - R12 * y);
            double delZKappa = S * (R23 * x - R13 * y);

            A[pos, 2] = delXPhi;
            A[pos, 3] = delXKappa;
            A[pos, 0] = delXScale;
            A[pos, 4] = 1;
            A[pos + 1, 1] = delYOmega;
            A[pos + 1, 2] = delYPhi;
            A[pos + 1, 3] = delYKappa;
            A[pos + 1, 0] = delYScale;
            A[pos + 1, 5] = 1;
            A[pos + 2, 1] = delZOmega;
            A[pos + 2, 2] = delZPhi;
            A[pos + 2, 3] = delZKappa;
            A[pos + 2, 0] = delZScale;
            A[pos + 2, 6] = 1;


        }
        //AppShell.Current.DisplayAlert("Res", $"{A}", "OK");
        return A;
    }

    public List<Matrix<double>> CalcPointsList = new List<Matrix<double>>();
    public void CalcPoints()
    {
        CalcPointsList.Clear();
        for (int i = 0; i < FromCoords.Count; i++)
        {
            //var point = T + S * R * FromCoords[i];
            //var point = T + S * R * FromCoords[i];
            var x = FromCoords[i][0, 0];
            var y = FromCoords[i][1, 0];
            var z = FromCoords[i][2, 0];
            Double X = S * (R11 * x + R21 * y + R31 * z) + T[0, 0];
            Double Y = S * (R12 * x + R22 * y + R32 * z) + T[1, 0];
            Double Z = S * (R13 * x + R23 * y + R33 * z) + T[2, 0];

            Matrix<double> point = DenseMatrix.OfArray(new double[,]
                {
                {X},
                {Y},
                {Z}
                });

            CalcPointsList.Add(point);
            // AppShell.Current.DisplayAlert("Res", $"s {S} {R} {T}", "OK");
            //AppShell.Current.DisplayAlert("Error", $"{point}", "OK");
        }
    }





    public Matrix<double> L;

    public Matrix<double> Misclosure()
    {

        var rows = FromCoords.Count * 3;
        L = Matrix<double>.Build.Dense(rows, 1);
        for (int i = 0; i < FromCoords.Count; i++)
        {
            var pos = i * 3;
            var deltaL = ToCoords[i] - CalcPointsList[i];
            //var deltaL = ToCoordsBar[i] - CalcPointsList[i];
            L[pos, 0] = deltaL[0, 0];
            L[pos + 1, 0] = deltaL[1, 0];
            L[pos + 2, 0] = deltaL[2, 0];
            //AppShell.Current.DisplayAlert("Res", $" {ToCoordsBar[0]} {ToCoordsBar[1]} {ToCoordsBar[2]} {ToCoordsBar[3]}", "OK");

        }
        //AppShell.Current.DisplayAlert("Res", $"{L}", "OK");
        return L;
    }

    Matrix<double> T;

    List<Matrix<double>> Transformed;

    public List<Matrix<double>> Rollout()
    {
        var s = new List<Matrix<double>>();
        InitialApproximations();
        for (int i = 0; i < ToTransform.Count; i++)
        {
            Weights(ToTransform[i]); //AppShell.Current.DisplayAlert("Error", $"{ToTransform[i]}", "OK");
            do
            {
                GetParameters();
                Misclosure();

                CorrectionVector = ((A.Transpose().Multiply(A)).Inverse()) * (A.Transpose().Multiply(L));
                ParameterVector = ParameterVector + CorrectionVector;
                S = ParameterVector[0, 0];
                Omega = ParameterVector[1, 0];
                Phi = ParameterVector[2, 0];
                Kappa = ParameterVector[3, 0];
                T[0, 0] = ParameterVector[4, 0];
                T[1, 0] = ParameterVector[5, 0];
                T[2, 0] = ParameterVector[6, 0];
            }
            while (Math.Abs(CorrectionVector[0, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[1, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[2, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[3, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[4, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[5, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[6, 0]) > 0.000001
    );

        }

        for (int i = 0; i < ToTransform.Count; i++)
        {

            var x = ToTransform[i][0, 0];
            var y = ToTransform[i][1, 0];
            var z = ToTransform[i][2, 0];
            Double X = S * (R11 * x + R21 * y + R31 * z) + T[0, 0];
            Double Y = S * (R12 * x + R22 * y + R32 * z) + T[1, 0];
            Double Z = S * (R13 * x + R23 * y + R33 * z) + T[2, 0];

            Matrix<double> point = DenseMatrix.OfArray(new double[,]
                {
                {X},
                {Y},
                {Z}
                });
            s.Add(point);
        }
        Misclosure();
        return s;
    }

    public List<Matrix<double>> RolloutIDW()
    {
        var s = new List<Matrix<double>>();
        InitialApproximations();
        for (int i = 0; i < ToTransform.Count; i++)
        {
            Weights(ToTransform[i]); //AppShell.Current.DisplayAlert("Error", $"{ToTransform[i]}", "OK");
            do
            {
                GetParameters();
                Misclosure();

                CorrectionVector = ((A.Transpose().Multiply(W).Multiply(A)).Inverse()) * (A.Transpose().Multiply(W).Multiply(L));
                ParameterVector = ParameterVector + CorrectionVector;
                S = ParameterVector[0, 0];
                Omega = ParameterVector[1, 0];
                Phi = ParameterVector[2, 0];
                Kappa = ParameterVector[3, 0];
                T[0, 0] = ParameterVector[4, 0];
                T[1, 0] = ParameterVector[5, 0];
                T[2, 0] = ParameterVector[6, 0];
            }
            while (Math.Abs(CorrectionVector[0, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[1, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[2, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[3, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[4, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[5, 0]) > 0.000001 &&
    Math.Abs(CorrectionVector[6, 0]) > 0.000001
    );

        }

        //Misclosure();

        for (int i = 0; i < ToTransform.Count; i++)
        {

            var x = ToTransform[i][0, 0];
            var y = ToTransform[i][1, 0];
            var z = ToTransform[i][2, 0];
            Double X = S * (R11 * x + R21 * y + R31 * z) + T[0, 0];
            Double Y = S * (R12 * x + R22 * y + R32 * z) + T[1, 0];
            Double Z = S * (R13 * x + R23 * y + R33 * z) + T[2, 0];

            Matrix<double> point = DenseMatrix.OfArray(new double[,]
                {
                {X},
                {Y},
                {Z}
                });
            //AppShell.Current.DisplayAlert("Res", $" {point}", "OK");
            s.Add(point);
        }
        Misclosure();
        //AppShell.Current.DisplayAlert("Error", $" {R}", "OK");
        return s;
    }



    Matrix<double> CorrectionVector = Matrix<double>.Build.Dense(7, 1);
    public Matrix<double> ParameterVector = Matrix<double>.Build.Dense(7, 1);

    public List<Matrix<double>> Transform3DIDW()
    {
        CalcPointsList.Clear();
        for (int i = 0; i < ToTransform.Count; i++)
        {

            var x = ToTransform[i][0, 0];
            var y = ToTransform[i][1, 0];
            var z = ToTransform[i][2, 0];
            Double X = S * (R11 * x + R21 * y + R31 * z) + T[0, 0];
            Double Y = S * (R12 * x + R22 * y + R32 * z) + T[1, 0];
            Double Z = S * (R13 * x + R23 * y + R33 * z) + T[2, 0];

            Matrix<double> point = DenseMatrix.OfArray(new double[,]
                {
                {Convert.ToDouble($"{X:F3}")},
                {Y},
                {Z}
                });
            //Transformed.Add(point);
            AppShell.Current.DisplayAlert("Res", $" {point}", "OK");
        }

        /*var ToMatrix = R.Multiply(FromMatrix).Multiply(S).Add(T);

        //AppShell.Current.DisplayAlert("Error", $" {ToMatrix.ToString()}", "OK");

        Easting = $"{ToMatrix[0, 0]:F3}";
        Northing = $"{ToMatrix[1, 0]:F3}";
        Elevation = $"{ToMatrix[2, 0]:F3}";*/

        return Transformed;
    }


    Matrix<double> ToX1 { get; set; }
    Matrix<double> ToX2;
    Matrix<double> ToX3;
    Matrix<double> Fromx1;
    Matrix<double> Fromx2;
    Matrix<double> Fromx3;

    public List<Matrix<double>> ToCoords = new List<Matrix<double>>();
    public List<Matrix<double>> FromCoords = new List<Matrix<double>>();
    public List<Matrix<double>> ToTransform = new List<Matrix<double>>();

    public (List<Matrix<double>> ToCoords, List<Matrix<double>> FromCoords, List<Matrix<double>> ToTransform) createInitialMatricesSVD(List<Coordinate> To, List<Coordinate> From, List<Coordinate> Transform)
    {
        ToCoords.Clear();
        FromCoords.Clear();
        ToTransform.Clear();
        foreach (Coordinate coordinate in To)
        {
            ToX = DenseMatrix.OfArray(new double[,]
        {
            {coordinate.x},
            {coordinate.y},
            {coordinate.z}
        });
            ToCoords.Add(ToX);
        }

        foreach (Coordinate coordinate in From)
        {
            FromX = DenseMatrix.OfArray(new double[,]
        {
            {coordinate.x},
            {coordinate.y},
            {coordinate.z}
        });
            FromCoords.Add(FromX);
        }

        foreach (Coordinate coordinate in Transform)
        {
            ToTransformX = DenseMatrix.OfArray(new double[,]
        {
            {coordinate.x},
            {coordinate.y},
            {coordinate.z}
        });
            //AppShell.Current.DisplayAlert("Error", $"ToTransform behind {ToTransform.Count}", "OK");
            ToTransform.Add(ToTransformX);
        }

        count = ToCoords.Count;
        /*AppShell.Current.DisplayAlert("Error", $"ToCoords {ToCoords.Count}", "OK");
        AppShell.Current.DisplayAlert("Error", $"FromCoords {FromCoords.Count}", "OK");
        AppShell.Current.DisplayAlert("Error", $"ToTransform {ToTransform.Count}", "OK");*/

        return (ToCoords, FromCoords, ToTransform);
    }

    Matrix<double> ToCentroid;
    Matrix<double> FromCentroid;

    public void CentroidMatrices()
    {
        ToCentroid = (ToCoords[0] + ToCoords[1] + ToCoords[2]) * 1 / 3;
        FromCentroid = (FromCoords[0] + FromCoords[1] + FromCoords[2]) * 1 / 3;

    }
    /*public void CentroidMatrices()
    {
        ToCentroid = (ToCoords[0] + ToCoords[1] + ToCoords[2]) * 1 / 3;
        FromCentroid = (fro + Fromx2 + Fromx1) * 1 / 3;

    }*/

    Matrix<double> ToX1Bar;
    Matrix<double> ToX2Bar;
    Matrix<double> ToX3Bar;
    Matrix<double> Fromx1Bar;
    Matrix<double> Fromx2Bar;
    Matrix<double> Fromx3Bar;

    /*public void CentredCoordinates()
    {
        ToX1Bar = ToX1 - ToCentroid;
        ToX2Bar = ToX2 - ToCentroid;
        ToX3Bar = ToX3 - ToCentroid;
        Fromx1Bar = Fromx1 - FromCentroid;
        Fromx2Bar = Fromx2 - FromCentroid;
        Fromx3Bar = Fromx3 - FromCentroid;

    }*/

    public void CentredCoordinates()
    {
        FromCoordsBar.Clear();
        ToCoordsBar.Clear();
        foreach (Matrix<double> uncenteredFrom in FromCoords)
        {
            var centeredFrom = uncenteredFrom - FromCentroid;
            FromCoordsBar.Add(centeredFrom);
            //AppShell.Current.DisplayAlert("Res", $"delta {centeredFrom}", "OK");
        }

        foreach (Matrix<double> uncenteredTo in ToCoords)
        {
            var centeredTo = uncenteredTo - ToCentroid;
            ToCoordsBar.Add(centeredTo);
            //AppShell.Current.DisplayAlert("Res", $"delta {centeredTo}", "OK");
        }
    }

    Matrix<double> H;

    public void CrossCovarianceMatrix()
    {
        H = (FromCoordsBar[0] * ToCoordsBar[0].Transpose()) + (FromCoordsBar[1] * ToCoordsBar[1].Transpose()) + (FromCoordsBar[2] * ToCoordsBar[2].Transpose());
    }

    //Matrix<double> R;

    public Matrix<double> SVD()
    {

        CentroidMatrices();
        CentredCoordinates();
        CrossCovarianceMatrix();

        var svd = H.Svd(true);
        R = svd.VT.Transpose() * svd.U.Transpose();

        return R;
    }

    //public Double S { get; set; }

    /*public Double S3D()
    {
        var To = ToX1Bar.Transpose() * R.Multiply(Fromx1Bar) + ToX2Bar.Transpose() * R.Multiply(Fromx2Bar) + ToX3Bar.Transpose() * R.Multiply(Fromx3Bar);
        var From = Fromx1Bar.Transpose() * Fromx1Bar + Fromx2Bar.Transpose() * Fromx2Bar + Fromx3Bar.Transpose() * Fromx3Bar;

        var SumTo = To[0, 0]; //+ To[0, 1] + To[0, 2];
        var SumFrom = From[0, 0];// + From[1, 0] + From[2, 0];


        S = SumTo / SumFrom;

        return S;
    }*/
    public Double S3D()
    {
        var To = ToCoordsBar[0].Transpose() * R.Multiply(FromCoordsBar[0]) + ToCoordsBar[1].Transpose() * R.Multiply(FromCoordsBar[1]) + ToCoordsBar[2].Transpose() * R.Multiply(FromCoordsBar[2]);
        var From = FromCoordsBar[0].Transpose() * FromCoordsBar[0] + FromCoordsBar[1].Transpose() * FromCoordsBar[1] + FromCoordsBar[2].Transpose() * FromCoordsBar[2];

        var SumTo = To[0, 0]; //+ To[0, 1] + To[0, 2];
        var SumFrom = From[0, 0];// + From[1, 0] + From[2, 0];


        S = SumTo / SumFrom;

        return S;
    }

    //Matrix<double> T;

    public Matrix<double> Translation()
    {

        T = ToCentroid - S * R * FromCentroid;

        return T;

    }

    public void CalcPointsSVD()
    {
        CalcPointsList.Clear();
        for (int i = 0; i < FromCoords.Count; i++)
        {
            //var point = T + S * R * FromCoords[i];
            //var point = T + S * R * FromCoords[i];
            var x = FromCoords[i][0, 0];
            var y = FromCoords[i][1, 0];
            var z = FromCoords[i][2, 0];
            Double X = S * (R11 * x + R21 * y + R31 * z) + T[0, 0];
            Double Y = S * (R12 * x + R22 * y + R32 * z) + T[1, 0];
            Double Z = S * (R13 * x + R23 * y + R33 * z) + T[2, 0];

            var point = R.Multiply(FromCoords[i]).Multiply(S).Add(T);

            CalcPointsList.Add(point);
            // AppShell.Current.DisplayAlert("Res", $"s {S} {R} {T}", "OK");
            //AppShell.Current.DisplayAlert("Error", $"{point}", "OK");
        }
    }

    public List<Matrix<double>> Transform3D()
    {

        var s = new List<Matrix<double>>();
        var rotation = SVD();
        R11 = R[0, 0];
        R12 = R[0, 1];
        R13 = R[0, 2];
        R21 = R[1, 0];
        R22 = R[1, 1];
        R23 = R[1, 2];
        R31 = R[2, 0];
        R32 = R[2, 1];
        R33 = R[2, 2];
        S3D();
        Translation();
        //AppShell.Current.DisplayAlert("Result", $"{H} {ToCentroid} {FromCentroid}", "OK");
        for (int i = 0; i < ToTransform.Count; i++)
        {

            var x = ToTransform[i][0, 0];
            var y = ToTransform[i][1, 0];
            var z = ToTransform[i][2, 0];
            //AppShell.Current.DisplayAlert("Result", $"{R11 * x} {R21 * y} {R31 * z}", "OK");
            Double X = S * (R11 * x + R21 * y + R31 * z) + T[0, 0];
            Double Y = S * (R12 * x + R22 * y + R32 * z) + T[1, 0];
            Double Z = S * (R13 * x + R23 * y + R33 * z) + T[2, 0];
            //AppShell.Current.DisplayAlert("Result", $"{R11}", "OK");
            var point = R.Multiply(ToTransform[i]).Multiply(S).Add(T);

            /*Matrix<double> point = DenseMatrix.OfArray(new double[,]
                {
                {X},
                {Y},
                {Z}
                });*/
            //AppShell.Current.DisplayAlert("Res", $" {point}", "OK");
            s.Add(point);

        }



        /*Phi = Math.Asin(R31);
        Kappa = Math.Acos(R11 / Math.Cos(Phi));
        Omega = Math.Acos(R33 / Math.Cos(Phi));*/

        ParameterVector[0, 0] = S;
        ParameterVector[1, 0] = Omega;
        ParameterVector[2, 0] = Phi;
        ParameterVector[3, 0] = Kappa;
        ParameterVector[4, 0] = T[0, 0];
        ParameterVector[5, 0] = T[1, 0];
        ParameterVector[6, 0] = T[2, 0];
        CalcPointsSVD();
        Misclosure();

        //var ToMatrix = R.Multiply(FromMatrix).Multiply(S).Add(T);
        //s.Add(ToMatrix);

        //AppShell.Current.DisplayAlert("Error", $" {R}", "OK");

        /*Easting = $"{ToMatrix[0, 0]:F3}";
        Northing = $"{ToMatrix[1, 0]:F3}";
        Elevation = $"{ToMatrix[2, 0]:F3}";*/

        return s;
    }

    public (string Easting, string Northing, string Elevation) Transform3DQuick(string E7, string N7, string Z7)
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

    double XCart;
    double YCart;
    double ZCart;

    /*public GetCartesian()
    {
        v = a / root(1 - e2 sinphi2);
        e2 = 2f - f2;

        XCart = (v + h) * CosPhi CosLambda;
        YCart = (v + h) * CosPhi SinLambda;
        ZCart = (v * (1 - e2) + h) Sinphi;
    }*/

}

