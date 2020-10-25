// cracks.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <Windows.h>
#include "opencv2/core/types_c.h"
#include "opencv/cv.hpp"
#include "opencv2/core/core.hpp"
#include "opencv2/imgproc/imgproc.hpp"
#include "opencv2/highgui/highgui.hpp"
#include <stdlib.h>
#include <iostream>
#include <math.h>
#include <malloc.h>

#define ROUND(a) a+0.5

using namespace cv;
using namespace std;

/** Function Headers */
int Initiate();
void Transformation(Mat&, Mat&);
void Classification(Mat&, Mat&);
void Identification(Mat&, Mat&, Mat&);
int Window(Mat&, Mat&);
void Removal(int&, Mat&, Mat&, Mat&, Mat&);
void Sharpness(Mat&);


/** @function main */

int main()
{
	int r = Initiate();

	return r;
}


/** @function Main_module */

int Initiate()
{
	Mat img, src, dst, cracks;

	char* or = "Original Image";
	char* fin = "Final Image";

	/// Load an image
	char pic[20] = "monalisa.jpg";
	/*cout << "enter an image: ";
	cin >> pic;*/
	img = imread(pic);

	//namedWindow(or, CV_WINDOW_AUTOSIZE);

	/*if (!img.data)
	{
		cout << "image not found...";
		Sleep(4300);
		return -1;
	}
	else
		imshow(or, img);*/
	cout << endl;
	cout << "image size= " << img.cols << " X " << img.rows << endl;

	cout << "the output will be stored in the same directory where the original image is.\n\n" << endl;

	cvtColor(img, src, CV_BGR2GRAY);

	Transformation(src, dst);

	Classification(src, dst);

	Identification(img, cracks, dst);

	int max = Window(img, cracks);

	int size = ROUND(((float)max * (30.0 / 100.0))) + max;
	if (size % 2 == 0)
		size++;

	Removal(size, img, src, cracks, dst);

	Sharpness(src);

	int k = 0, k1 = 0;
	char pic1[20];
	try {
		while (pic[k] != '.') {
			pic1[k1] = pic[k];
			k++;
			k1++;
		}
		pic1[k1] = '1';
		k1++;
		do {
			pic1[k1] = pic[k];
			k1++;
			k++;
		} while (pic[k - 1] != '\0');
		imwrite(pic1, src);
	}
	catch (...) {}

	//namedWindow(fin, CV_WINDOW_AUTOSIZE);
	//imshow(fin, src);

	waitKey(0);

	/*cvDestroyWindow(or);
	cvDestroyWindow(fin);*/

	return 1;
}

/** @function Morphology_Operation */

void Transformation(Mat& src, Mat& dst)
{
	// Since MORPH_X : 5 = Top Hat. 6 = Black Hat

	Mat element = getStructuringElement(0, Size(5, 5), Point(-1, -1));
	morphologyEx(src, dst, 6, element);
	//imwrite("D:\\OpenCV\\cracks\\cracks\\Results\\transform.jpg", dst);

}


/** @function Brush_strokes */

void Classification(Mat& src, Mat& dst)
{
	/* 0: Binary
	1: Binary Inverted
	2: Threshold Truncated
	3: Threshold to Zero
	4: Threshold to Zero Inverted
	*/

	/* 0 = black
	255 = white
	*/

	src = dst;
	threshold(src, dst, 30, 255, 0);
	//	adaptiveThreshold(src, dst, 255, ADAPTIVE_THRESH_MEAN_C, THRESH_BINARY_INV, 5, 10);

}


/* @function crack pixel storage */

void Identification(Mat &img, Mat& cracks, Mat& dst)
{
	int i, j, k;
	Mat hs;
	Vec3f intensity;

	findNonZero(dst, cracks);
	cout << endl;
	cout << "total no of cracks= " << cracks.total() << endl;
	cout << "\nProcessing...\n" << endl;
	//	i=cracks.at<Point>(0).x;
	//	j=cracks.at<Point>(0).y;
	Mat t;
	//	cvtColor( img, t, CV_BGR2GRAY );
	cvtColor(img, hs, CV_BGR2HSV);
	imwrite("D:\\OpenCV\\cracks\\cracks\\Results\\separation.jpg", hs);
	//	vector<Mat> intensity;
	//	split( hs, intensity );

	/*	for (long int k = 25300; k < 25330; k++) {
	i = cracks.at<Point>(k).x;
	j = cracks.at<Point>(k).y;
	dst.at<uchar>(j, i) = 150;
	intensity = hs.at<Vec3b>(j, i);
	cout << intensity.val[0] << " " << intensity.val[1] << " " << intensity.val[2] << endl;
	}     */


	for (long int k = 0; k < cracks.total(); k++) {
		i = cracks.at<Point>(k).x;
		j = cracks.at<Point>(k).y;
		intensity = hs.at<Vec3b>(j, i);
		if (intensity.val[0] <= 50 && intensity.val[1] <= 100 && intensity.val[2] >= 200)
			dst.at<uchar>(j, i) = 0;
	}

	findNonZero(dst, cracks);

	//imwrite("D:\\OpenCV\\cracks\\cracks\\Results\\classify.jpg", dst);

	/*	for (int i=50; i<=450; i++) {
	Vec3f intensity=hs.at<Vec3b>(50, i);
	cout<<intensity.val[0]<<" "<<intensity.val[1]<<" "<<intensity.val[2]<<endl;
	}       */
}



/* @function find window size */

int Window(Mat& img, Mat& cracks)
{
	int c = 0, max = 0;

	for (unsigned k = 0; k < cracks.total(); k++) {

		try {
			while ((cracks.at<Point>(k).x) + 1 == cracks.at<Point>(k + 1).x && cracks.at<Point>(k).y == cracks.at<Point>(k + 1).y && cracks.at<Point>(k).x < img.cols && cracks.at<Point>(k).y < img.rows) {
				c++;
				k++;
				if (c > 6) {
					c = 0;
					while ((cracks.at<Point>(k).x) + 1 == cracks.at<Point>(k + 1).x && cracks.at<Point>(k).y == cracks.at<Point>(k + 1).y && cracks.at<Point>(k).x < img.cols && cracks.at<Point>(k).y < img.rows)
						k++;
					break;
				}
			}
		}
		catch (...) {}
		k++;

		if (c > max)
			max = c;

		c = 0;

	}

	/*		for (int i = 0; i < dst.cols; i++ ) {
	for (int j = 0; j < dst.rows; j++) {
	if (dst.at<uchar>(j, i) == 255) {
	cout << i << ", " << j << endl;
	}
	}
	}         */

	/*    for (unsigned i = 0; i < cracks.total(); i++ ) {
	cout << "Zero#" << i << ": " << cracks.at<Point>(i).x << ", " << cracks.at<Point>(i).y << endl;
	}   */
	//        printf(" i: %u \t j: %u \n",cracks.at<Point>(i).x, cracks.at<Point>(i).y); 


	return max;
}


/* @function crack removal */

void Removal(int& size1, Mat& img, Mat& src, Mat& cracks, Mat& dst)
{
	int i = 0, j = 0, c, k = 1, med, m, n, temp, ctr, len, size, l;
	int b[250], g[250], r[250];
	Vec3b intensity, intensity1, intensity2;
	src = img;
	size = size1;

	/*	Vec3f intensity=img.at<Vec3b>(0, 130);
	cout<<intensity.val[0]<<endl;
	cout<<intensity.val[1]<<endl;
	cout<<intensity.val[2]<<endl;     */

	//		cout << "window size= " << size << endl;
	cout << "total= " << cracks.total() << endl;

	//	Mat dst1=imread("monalisa.jpg", 0);
	//	normalize(dst, dst1, 0, 255, NORM_MINMAX, CV_8UC1);
	//	cout<<"in= "<<(int)dst.at<unsigned char>(0,81);

	//	for(int i=139; i<=170; i++)
	//		dst.at<uchar>(Point(i, 0)) = 150;

	/*	unsigned char *input=(unsigned char*)(dst.data);
	int r=input[200];
	cout<<"r= "<<r;

	if (dst.at<uchar>(0, 78) == 255)
	cout << "crack";
	else
	cout << "not a crack";
	*/

	l = size / 2;
	int min, ptx1, ptx2, ptx3, ptx4, pty1, pty2, pty3, pty4, flag = 1, no;

	while (++k < 6) {
		for (long int c = 0; c < cracks.total(); c++) {
			flag = 1;
			if (((cracks.at<Point>(c).x) - l) < 0 || ((cracks.at<Point>(c).x) + l) >= img.cols || ((cracks.at<Point>(c).y) - l) < 0 || ((cracks.at<Point>(c).y) + l) >= img.rows)
				continue;
			if (flag == 1 && k == 2) {
				min = 20;
				for (i = 1; i <= l; i++) {
					if (dst.at<uchar>((cracks.at<Point>(c).y), ((cracks.at<Point>(c).x) - i)) != 255) {
						ptx1 = cracks.at<Point>(c).x - i;
						pty1 = cracks.at<Point>(c).y;
						break;
					}
				}
				if (i <= l) {
					for (j = 1; j <= l; j++) {
						if (dst.at<uchar>((cracks.at<Point>(c).y), ((cracks.at<Point>(c).x) + j)) != 255) {
							ptx2 = cracks.at<Point>(c).x + j;
							pty2 = cracks.at<Point>(c).y;
							break;
						}
					}
				}
				if (j <= l && i <= l)
					min = i + j + 1;

				for (i = 1; i <= l; i++) {
					if (dst.at<uchar>(((cracks.at<Point>(c).y) - i), (cracks.at<Point>(c).x)) != 255) {
						ptx3 = cracks.at<Point>(c).x;
						pty3 = cracks.at<Point>(c).y - i;
						break;
					}
				}
				if (i <= l) {
					for (j = 1; j <= l; j++) {
						if (dst.at<uchar>(((cracks.at<Point>(c).y) + j), (cracks.at<Point>(c).x)) != 255) {
							ptx4 = cracks.at<Point>(c).x;
							pty4 = cracks.at<Point>(c).y + j;
							break;
						}
					}
				}

				if ((i > l || j > l) && min == 20) {

				}
				else
					if (i + j + 1 > min) {
						intensity1 = src.at<Vec3b>(pty1, ptx1);
						intensity2 = src.at<Vec3b>(pty2, ptx2);
						if ((abs(intensity2.val[0] - intensity1.val[0]) <= 40) && (abs(intensity2.val[1] - intensity1.val[1]) <= 40) && (abs(intensity2.val[2] - intensity1.val[2]) <= 40)) {
							src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[0] = (intensity1.val[0] + intensity2.val[0]) / 2;
							src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[1] = (intensity1.val[1] + intensity2.val[1]) / 2;
							src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[2] = (intensity1.val[2] + intensity2.val[2]) / 2;
							dst.at<uchar>(cracks.at<Point>(c).y, cracks.at<Point>(c).x) = 0;
						}
					}
					else {
						intensity1 = src.at<Vec3b>(pty3, ptx3);
						intensity2 = src.at<Vec3b>(pty4, ptx4);
						if ((abs(intensity2.val[0] - intensity1.val[0]) <= 40) && (abs(intensity2.val[1] - intensity1.val[1]) <= 40) && (abs(intensity2.val[2] - intensity1.val[2]) <= 40)) {
							src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[0] = (intensity1.val[0] + intensity2.val[0]) / 2;
							src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[1] = (intensity1.val[1] + intensity2.val[1]) / 2;
							src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[2] = (intensity1.val[2] + intensity2.val[2]) / 2;
							dst.at<uchar>(cracks.at<Point>(c).y, cracks.at<Point>(c).x) = 0;
						}
					}
			}
		}


		findNonZero(dst, cracks);

		cout << cracks.total() << endl;
		len = size;
		for (long int c = 0; c < cracks.total(); c++) {
			ctr = 0;
			size = len;
			l = size / 2;
			while (1) {
				no = 0;
				for (i = (cracks.at<Point>(c).x) - l; i <= (cracks.at<Point>(c).x) + l; i++) {
					if (i < 0 || i >= img.cols)
						continue;
					for (j = (cracks.at<Point>(c).y) - l; j <= (cracks.at<Point>(c).y) + l; j++) {
						if (j < 0 || j >= img.rows)
							continue;
						if (dst.at<uchar>(j, i) == 0)
							no++;
					}
				}
				if ((((float)no / (float)(size * size)) * 100) > 60 && size > 4) {
					//						cout << no << " " << size * size << endl;
					size -= 2;
					l = size / 2;
				}
				else
					break;
			}
			for (i = (cracks.at<Point>(c).x) - l; i <= (cracks.at<Point>(c).x) + l; i++) {
				if (i < 0 || i >= img.cols)
					continue;
				for (j = (cracks.at<Point>(c).y) - l; j <= (cracks.at<Point>(c).y) + l; j++) {
					if (j < 0 || j >= img.rows)
						continue;
					if (dst.at<uchar>(j, i) == 0) {
						intensity = src.at<Vec3b>(j, i);
						b[ctr] = intensity.val[0];
						g[ctr] = intensity.val[1];
						r[ctr] = intensity.val[2];
						ctr++;
						b[ctr] = intensity.val[0];
						g[ctr] = intensity.val[1];
						r[ctr] = intensity.val[2];
						ctr++;
						b[ctr] = intensity.val[0];
						g[ctr] = intensity.val[1];
						r[ctr] = intensity.val[2];
						ctr++;
					}
					else {
						intensity = src.at<Vec3b>(j, i);
						b[ctr] = intensity.val[0];
						g[ctr] = intensity.val[1];
						r[ctr] = intensity.val[2];
						ctr++;
					}
				}
			}

			for (m = 0; m < ctr - 1; m++) {
				for (n = m + 1; n<ctr; n++) {
					if (((b[m] + g[m] + r[m]) / 3)>((b[n] + g[n] + r[n]) / 3)) {

						temp = b[m];
						b[m] = b[n];
						b[n] = temp;

						temp = g[m];
						g[m] = g[n];
						g[n] = temp;

						temp = r[m];
						r[m] = r[n];
						r[n] = temp;
					}
				}
			}

			if (ctr % 2 == 1) {
				med = (ctr / 2) + 1;
				src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[0] = b[med];
				src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[1] = g[med];
				src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[2] = r[med];
			}
			else {
				med = ctr / 2;
				src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[0] = ((b[med] + b[med + 1]) / 2);
				src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[1] = ((g[med] + g[med + 1]) / 2);
				src.at<Vec3b>(cracks.at<Point>(c).y, cracks.at<Point>(c).x).val[2] = ((r[med] + r[med + 1]) / 2);
			}
		}  

	}
	//	namedWindow( fin, CV_WINDOW_AUTOSIZE );
	//	imshow( fin, src );

	//imwrite("D:\\OpenCV\\cracks\\cracks\\Results\\removed.jpg", src);

}



/* @function image sharpening */

void Sharpness(Mat& src)
{
	Mat tmp = src;

	//	bilateralFilter(tmp, src, 9, 30, 30);

	Mat src1 = src;
	GaussianBlur(src1, tmp, Size(1, 1), 1);
	addWeighted(src1, 1.5, tmp, -0.5, -10.0, src);

	//imwrite("D:\\OpenCV\\cracks\\cracks\\Results\\final.jpg", src);
}
