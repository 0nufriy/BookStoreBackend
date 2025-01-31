# BookStoreBackend

BookStoreBackend is a backend application for managing a bookstore, developed in C# using Entity Framework.

## Requirements

- .NET SDK version 8.0 or later
- Database server (e.g., SQL Server, PostgreSQL)

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/0nufriy/BookStoreBackend.git
   cd BookStoreBackend
   ```

2. Open the solution in your preferred development environment (e.g., Visual Studio, Rider).

3. Configure the database connection string in `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "YourConnectionStringHere"
     }
   }
   ```

4. Apply migrations to create the necessary tables in the database:

   ```bash
   dotnet ef database update
   ```

5. Run the application through the IDE

## Usage

Once the application is running, it will be available at `https://localhost:7109` (or another port specified in the settings). You can interact with the API using tools like Postman or cURL.
# BookStoreBackend API Specification

This document outlines the API specification for the BookStoreBackend application, as described in the Swagger UI documentation.

## Authentication

### POST /api/Auth/login

* **Description:** Authenticates a user and returns user information along with a token.
* **Request Body (application/json):**
  ```json
  {
    "login": "string",
    "password": "string"
  }
  ```
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "login": "string",
    "email": "string",
    "phone": "string",
    "name": "string",
    "role": "string",
    "token": "string",
    "addresses": [
      {
        "id": 0,
        "adressName": "string",
        "adress": "string",
        "userId": 0
      }
    ]
  }
  ```

### POST /api/Auth/registr

* **Description:** Registers a new user.
* **Request Body (application/json):**
  ```json
  {
    "login": "string",
    "password": "string",
    "email": "string",
    "phone": "string",
    "name": "string"
  }
  ```
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "login": "string",
    "email": "string",
    "phone": "string",
    "name": "string",
    "role": "string",
    "token": "string",
    "addresses": [
      {
        "id": 0,
        "adressName": "string",
        "adress": "string",
        "userId": 0
      }
    ]
  }
  ```

### POST /api/Auth/registrAdmin

* **Description:** Registers a new admin user.
* **Request Body (application/json):**
  ```json
  {
    "login": "string",
    "password": "string",
    "email": "string",
    "phone": "string",
    "name": "string"
  }
  ```
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "login": "string",
    "email": "string",
    "phone": "string",
    "name": "string",
    "role": "string",
    "token": "string",
    "addresses": [
      {
        "id": 0,
        "adressName": "string",
        "adress": "string",
        "userId": 0
      }
    ]
  }
  ```

## Book Endpoints

### GET /api/Book

* **Description:** Retrieves a list of books.
* **Response 200 OK (text/plain):**
  ```json
  [
    {
      "id": 0,
      "genreId": 0,
      "bookName": "string",
      "autor": "string",
      "description": "string",
      "pagecount": 0,
      "format": "string",
      "isbn": "string",
      "cover": "string",
      "image": "string",
      "price": 0,
      "stock": 0,
      "gener": {
        "id": 0,
        "genreName": "string"
      },
      "comments": [
        {
          "commentId": 0,
          "bookId": 0,
          "userId": 0,
          "message": "string",
          "user": {
            "id": 0,
            "login": "string",
            "password": "string",
            "email": "string"
          }
        }
      ]
    }
  ]
  ```

### POST /api/Book

* **Description:** Creates a new book.
* **Request Body (application/json):**
  ```json
  {
    "genreId": 0,
    "bookName": "string",
    "autor": "string",
    "description": "string",
    "pagecount": 0,
    "format": "string",
    "isbn": "string",
    "cover": "string",
    "image": "string",
    "price": 0,
    "stock": 0
  }
  ```
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "genreId": 0,
    "bookName": "string",
    "autor": "string",
    "description": "string",
    "pagecount": 0,
    "format": "string",
    "isbn": "string",
    "cover": "string",
    "image": "string",
    "price": 0,
    "stock": 0,
    "gener": {
      "id": 0,
      "genreName": "string"
    },
    "comments": [
      {
        "commentId": 0,
        "bookId": 0,
        "userId": 0,
        "message": "string",
        "user": {
          "id": 0,
          "login": "string",
          "password": "string",
          "email": "string"
        }
      }
    ]
  }
  ```

### PUT /api/Book

* **Description:** Updates an existing book.
* **Request Body (application/json):**
  ```json
  {
    "id": 0,
    "genreId": 0,
    "bookName": "string",
    "autor": "string",
    "description": "string",
    "pagecount": 0,
    "format": "string",
    "isbn": "string",
    "cover": "string",
    "image": "string",
    "price": 0,
    "stock": 0
  }
  ```
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "genreId": 0,
    "bookName": "string",
    "autor": "string",
    "description": "string",
    "pagecount": 0,
    "format": "string",
    "isbn": "string",
    "cover": "string",
    "image": "string",
    "price": 0,
    "stock": 0,
    "gener": {
      "id": 0,
      "genreName": "string"
    },
    "comments": [
      {
        "commentId": 0,
        "bookId": 0,
        "userId": 0,
        "message": "string",
        "user": {
          "id": 0,
          "login": "string",
          "password": "string",
          "email": "string"
        }
      }
    ]
  }
  ```

### GET /api/Book/getSome/{count}/{iter}

* **Description:** Retrieves a limited number of books based on count and iteration parameters.
* **Parameters:**
    * `count` (path, integer): Number of books to retrieve.
    * `iter` (path, integer): Iteration parameter (likely for pagination).
* **Response 200 OK (text/plain):**
  ```json
  [
    {
      "id": 0,
      "genreId": 0,
      "bookName": "string",
      "autor": "string",
      "description": "string",
      "pagecount": 0,
      "format": "string",
      "isbn": "string",
      "cover": "string",
      "image": "string",
      "price": 0,
      "stock": 0,
      "gener": {
        "id": 0,
        "genreName": "string"
      },
      "comments": [
        {
          "commentId": 0,
          "bookId": 0,
          "userId": 0,
          "message": "string",
          "user": {
            "id": 0,
            "login": "string",
            "password": "string",
            "email": "string"
          }
        }
      ]
    }
  ]
  ```

### GET /api/Book/getCataloge

* **Description:** Retrieves a catalog of books, with filtering and pagination options.
* **Query Parameters:**
    * `count` (integer): Number of items per page.
    * `iter` (integer): Page number (iteration).
    * `minPrice` (integer): Minimum price filter.
    * `maxPrice` (integer): Maximum price filter.
    * `genreld` (integer): Genre ID filter.
    * `sort` (string): Sorting parameter.
    * `search` (string): Search term.
* **Response 200 OK (text/plain):**
  ```json
  [
    {
      "id": 0,
      "genreId": 0,
      "bookName": "string",
      "autor": "string",
      "description": "string",
      "pagecount": 0,
      "format": "string",
      "isbn": "string",
      "cover": "string",
      "image": "string",
      "price": 0,
      "stock": 0,
      "gener": {
        "id": 0,
        "genreName": "string"
      },
      "comments": [
        {
          "commentId": 0,
          "bookId": 0,
          "userId": 0,
          "message": "string",
          "user": {
            "id": 0,
            "login": "string",
            "password": "string",
            "email": "string"
          }
        }
      ]
    }
  ]
  ```

### GET /api/Book/genre

* **Description:** Retrieves a list of book genres.
* **Response 200 OK (text/plain):**
  ```json
  [
    {
      "id": 0,
      "genreId": 0,
      "bookName": "string",
      "autor": "string",
      "description": "string",
      "pagecount": 0,
      "format": "string",
      "isbn": "string",
      "cover": "string",
      "image": "string",
      "price": 0,
      "stock": 0,
      "gener": {
        "id": 0,
        "genreName": "string"
      },
      "comments": [
        {
          "commentId": 0,
          "bookId": 0,
          "userId": 0,
          "message": "string",
          "user": {
            "id": 0,
            "login": "string",
            "password": "string",
            "email": "string"
          }
        }
      ]
    }
  ]
  ```

### GET /api/Book/genre/five

* **Description:** Retrieves a list of five book genres.
* **Response 200 OK (text/plain):**
  ```json
  [
    {
      "id": 0,
      "genreId": 0,
      "bookName": "string",
      "autor": "string",
      "description": "string",
      "pagecount": 0,
      "format": "string",
      "isbn": "string",
      "cover": "string",
      "image": "string",
      "price": 0,
      "stock": 0,
      "gener": {
        "id": 0,
        "genreName": "string"
      },
      "comments": [
        {
          "commentId": 0,
          "bookId": 0,
          "userId": 0,
          "message": "string",
          "user": {
            "id": 0,
            "login": "string",
            "password": "string",
            "email": "string"
          }
        }
      ]
    }
  ]
  ```

### GET /api/Book/getOne/{id}

* **Description:** Retrieves a specific book by its ID.
* **Parameters:**
    * `id` (path, integer): Book ID.
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "genreId": 0,
    "bookName": "string",
    "autor": "string",
    "description": "string",
    "pagecount": 0,
    "format": "string",
    "isbn": "string",
    "cover": "string",
    "image": "string",
    "price": 0,
    "stock": 0,
    "gener": {
      "id": 0,
      "genreName": "string"
    },
    "comments": [
      {
        "commentId": 0,
        "bookId": 0,
        "userId": 0,
        "message": "string",
        "user": {
          "id": 0,
          "login": "string",
          "password": "string",
          "email": "string"
        }
      }
    ]
  }
  ```

### DELETE /api/Book/genre/{id}

* **Description:** Deletes a book genre by its ID.
* **Parameters:**
    * `id` (path, integer): Genre ID.
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "genreId": 0,
    "bookName": "string",
    "autor": "string",
    "description": "string",
    "pagecount": 0,
    "format": "string",
    "isbn": "string",
    "cover": "string",
    "image": "string",
    "price": 0,
    "stock": 0,
    "gener": {
      "id": 0,
      "genreName": "string"
    },
    "comments": [
      {
        "commentId": 0,
        "bookId": 0,
        "userId": 0,
        "message": "string",
        "user": {
          "id": 0,
          "login": "string",
          "password": "string",
          "email": "string"
        }
      }
    ]
  }
  ```

### DELETE /api/Book/{id}

* **Description:** Deletes a book by its ID.
* **Parameters:**
    * `id` (path, integer): Book ID.
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "genreId": 0,
    "bookName": "string",
    "autor": "string",
    "description": "string",
    "pagecount": 0,
    "format": "string",
    "isbn": "string",
    "cover": "string",
    "image": "string",
    "price": 0,
    "stock": 0,
    "gener": {
      "id": 0,
      "genreName": "string"
    },
    "comments": [
      {
        "commentId": 0,
        "bookId": 0,
        "userId": 0,
        "message": "string",
        "user": {
          "id": 0,
          "login": "string",
          "password": "string",
          "email": "string"
        }
      }
    ]
  }
  ```

### DELETE /api/Book/comment/{id}

* **Description:** Deletes a book comment by its ID.
* **Parameters:**
    * `id` (path, integer): Comment ID.
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "userName": "string",
    "message": "string"
  }
  ```

### POST /api/Book/comment

* **Description:** Adds a new comment to a book.
* **Request Body (application/json):**
  ```json
  {
    "bookId": 0,
    "message": "string"
  }
  ```
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "userName": "string",
    "message": "string"
  }
  ```

## Receipt Endpoints

### GET /api/Receipt

* **Description:** Retrieves a list of receipts.
* **Response 200 OK (text/plain):**
  ```json
  [
    {
      "id": 0,
      "userId": 0,
      "address": "string",
      "status": 0,
      "books": [
        {
          "id": 0,
          "bookName": "string",
          "autor": "string",
          "image": "string",
          "price": 0,
          "count": 0
        }
      ],
      "user": {
        "id": 0,
        "login": "string",
        "email": "string",
        "phone": "string",
        "name": "string",
        "role": "string",
        "addresses": [
          {
            "id": 0,
            "adressName": "string",
            "adress": "string"
          }
        ]
      }
    }
  ]
  ```

### POST /api/Receipt

* **Description:** Creates a new receipt (order).
* **Request Body (application/json):**
  ```json
  {
    "addressId": 0,
    "books": [
      {
        "bookId": 0,
        "count": 0
      }
    ]
  }
  ```
* **Response 200 OK (text/plain):**
  ```json
  [
    {
      "id": 0,
      "userId": 0,
      "address": "string",
      "status": 0,
      "books": [
        {
          "id": 0,
          "bookName": "string",
          "autor": "string",
          "image": "string",
          "price": 0,
          "count": 0
        }
      ],
      "user": {
        "id": 0,
        "login": "string",
        "email": "string",
        "phone": "string",
        "name": "string",
        "role": "string",
        "addresses": [
          {
            "id": 0,
            "adressName": "string",
            "adress": "string"
          }
        ]
      }
    }
  ]
  ```

### PUT /api/Receipt

* **Description:** Updates the status of a receipt.
* **Query Parameters:**
    * `receiptld` (integer): Receipt ID to update.
    * `status` (integer): New status value (Available values: 0, 1, 2, 3).
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "userId": 0,
    "address": "string",
    "status": 0,
    "books": [
      {
        "receiptId": 0,
        "bookId": 0,
        "count": 0,
        "book": {
          "id": 0,
          "genreId": 0,
          "bookName": "string",
          "autor": "string",
          "description": "string",
          "pagecount": 0,
          "format": "string",
          "isbn": "string",
          "cover": "string",
          "image": "string",
          "price": 0,
          "stock": 0,
          "gener": {
            "id": 0,
            "genreName": "string"
          },
          "comments": []
        }
      }
    ]
  }
  ```

### GET /api/Receipt/{id}

* **Description:** Retrieves a specific receipt by its ID.
* **Parameters:**
    * `id` (path, integer): Receipt ID.
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "userId": 0,
    "address": "string",
    "status": 0,
    "books": [
      {
        "id": 0,
        "bookName": "string",
        "autor": "string",
        "image": "string",
        "price": 0,
        "count": 0
      }
    ],
    "user": {
      "id": 0,
      "login": "string",
      "email": "string",
      "phone": "string",
      "name": "string",
      "role": "string",
      "addresses": [
        {
          "id": 0,
          "adressName": "string",
          "adress": "string"
        }
      ]
    }
  }
  ```

## User Endpoints

### GET /api/User/all

* **Description:** Retrieves a list of all users.
* **Response 200 OK (text/plain):**
  ```json
  [
    {
      "id": 0,
      "login": "string",
      "email": "string",
      "phone": "string",
      "name": "string",
      "role": "string",
      "addresses": [
        {
          "id": 0,
          "adressName": "string",
          "adress": "string",
          "userId": 0
        }
      ]
    }
  ]
  ```

### GET /api/User

* **Description:** Retrieves a list of users (potentially filtered/paginated).
* **Response 200 OK (text/plain):**
  ```json
  [
    {
      "id": 0,
      "login": "string",
      "email": "string",
      "phone": "string",
      "name": "string",
      "role": "string",
      "addresses": [
        {
          "id": 0,
          "adressName": "string",
          "adress": "string",
          "userId": 0
        }
      ]
    }
  ]
  ```

### PUT /api/User

* **Description:** Updates an existing user.
* **Request Body (application/json):**
  ```json
  {
    "login": "string",
    "email": "string",
    "phone": "string",
    "name": "string"
  }
  ```
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "login": "string",
    "email": "string",
    "phone": "string",
    "name": "string",
    "role": "string",
    "addresses": [
      {
        "id": 0,
        "adressName": "string",
        "adress": "string",
        "userId": 0
      }
    ]
  }
  ```

### DELETE /api/User

* **Description:** Deletes a user.
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "login": "string",
    "email": "string",
    "phone": "string",
    "name": "string",
    "role": "string",
    "addresses": [
      {
        "id": 0,
        "adressName": "string",
        "adress": "string",
        "userId": 0
      }
    ]
  }
  ```

### POST /api/User/address

* **Description:** Adds a new address to a user.
* **Request Body (application/json):**
  ```json
  {
    "adressName": "string",
    "city": "string",
    "street": "string",
    "house": "string",
    "postalCode": "string"
  }
  ```
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "login": "string",
    "email": "string",
    "phone": "string",
    "name": "string",
    "role": "string",
    "addresses": [
      {
        "id": 0,
        "adressName": "string",
        "adress": "string",
        "userId": 0
      }
    ]
  }
  ```

### DELETE /api/User/address/{addressId}

* **Description:** Deletes a user address by its ID.
* **Parameters:**
    * `addressId` (path, integer): Address ID.
* **Response 200 OK (text/plain):**
  ```json
  {
    "id": 0,
    "login": "string",
    "email": "string",
    "phone": "string",
    "name": "string",
    "role": "string",
    "addresses": [
      {
        "id": 0,
        "adressName": "string",
        "adress": "string",
        "userId": 0
      }
    ]
  }
  ```
