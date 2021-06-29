# TruyenAPI

Rest API was built with ASP.NET Core 3.0 with CRUD operations for a reading book website.
It is using Repository pattern to design architecture, communicating with database by ORM 
model.

## API (HTTP request)

Note:
1. [1] - Get all records
2. [2] - Get only one record.
3. [3] - Get record with foreign key.
4. {id}: Id of record which you want to get.
5. {key}: API key.

#### TacGia table

````console

Method: POST

URL: hostUrl/api/tacgia/{key}
JSON:
[
  {
    "TenTacGia": "",
    "TinhTrang": boolean
  }
  ,...
]

````

````console

Method: GET

[1] - URL: hostUrl/api/tacgia/{key}

[2] - URL: hostUrl/api/tacgia/{id}/{key}

[3] - URL: hostUrl/api/tacgia/{id}/{key}/details

````

````console

Method: PUT

URL: hostUrl/api/tacgia/{key}
JSON:
{
    "TenTacGia": "",
    "TinhTrang": boolean
}

````

````console

Method: DELETE

URL: url/api/tacgia/{id}/{key}

````

#### TheLoai table

````console

Method: POST

URL: hostUrl/api/theloai/{key}
JSON:
[
  {
    "TenTheLoai": "",
    "TinhTrang": boolean
  }
  ,...
]

````

````console

Method: GET

[1] - URL: hostUrl/api/theloai/{key}

[2] - URL: hostUrl/api/theloai/{id}/{key}

[3] - URL: hostUrl/api/theloai/{id}/{key}/details

````

````console

Method: PUT

URL: hostUrl/api/theloai/{key}
JSON:
{
    "TenTheLoai": "",
    "TinhTrang": boolean
}

````

````console

Method: DELETE

URL: url/api/theloai/{id}/{key}

````