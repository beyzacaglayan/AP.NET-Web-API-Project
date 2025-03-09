Overview

This project is an ASP.NET Web API application written in C#. It demonstrates the use of token-based authentication, data retrieval from an API, and the generation of PDF invoices using the ITextSharp library.

Authentication

To interact with the API, a token must first be obtained from the following endpoint:

Token Request URL:
http://istest.birfatura.net/

Credentials:

Username: test@test.com

Password: Test123

Data Retrieval

Once the token is obtained, it will be used to make a POST request to retrieve sales data:

API Endpoint:
http://istest.birfatura.net/api/test/SatislarGetir

The returned data will be stored in a list and displayed in a grid.

Grid Display

The grid will show all retrieved sales along with the sold products.

Each order in the grid will have an "Invoice" button.

PDF Generation

When the "Invoice" button is clicked, a PDF file containing the details of the corresponding order will be generated.

The design of the PDF is not crucial, but it should include all relevant order details.

The ITextSharp library will be used for PDF generation.

![görsel1](https://github.com/user-attachments/assets/5dafd57d-bcf6-4648-afe9-56adc6e39d04)

![görsel 2](https://github.com/user-attachments/assets/0247f1ce-3a0c-4e2a-8d8c-5384fadc09a7)



This project is an idea from BirFatura company.


