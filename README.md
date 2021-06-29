# TruyenAPI

Rest API was built with ASP.NET Core 3.0 with CRUD operations for a reading book website.
It is using Repository pattern to design architecture and communicating with database by
ORM model.

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
    "TenTacGia": string,
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
    "TenTacGia": string,
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
    "TenTheLoai": string,
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
    "TenTheLoai": string,
    "TinhTrang": boolean
}

````

````console

Method: DELETE

URL: url/api/theloai/{id}/{key}

````

#### Truyen table

````console

Method: POST

URL: hostUrl/api/truyen/{key}
JSON:
[
  {
    "TacGiaID": number,
    "TenTruyen": string,
    "MoTa": string,
    "TinhTrang": boolean,
    "HinhAnh": imageURL
  }
  ,...
]

````

````console

Method: GET

[1] - URL: hostUrl/api/truyen/{key}

[2] - URL: hostUrl/api/truyen/{id}/{key}

[3] - URL: hostUrl/api/truyen/{id}/{key}/details

````

````console

Method: PUT

URL: hostUrl/api/truyen/{key}
JSON:
{
    "TenTheLoai": string,
    "TinhTrang": boolean
}

````

````console

Method: DELETE

URL: url/api/truyen/{id}/{key}

````

#### PhuLuc table

````console

Method: POST

URL: hostUrl/api/phuluc/{key}
JSON:
[
  {
    "TruyenID": number,
    "TheLoaiID": number
  }
  ,...
]

````

````console

Method: GET

[1] - URL: hostUrl/api/phuluc{key}

[2] - URL: hostUrl/api/phuluc/{id}/{key}

````

````console

Method: PUT

URL: hostUrl/api/phuluc/{key}
JSON:
{
    "TruyenID": number,
    "TheLoaiID": number
}

````

````console

Method: DELETE

URL: url/api/phuluc/{id}/{key}

````

#### Chuong table

````console

Method: POST

URL: hostUrl/api/chuong/{key}
JSON:
[
  {
    "TruyenID": number,
    "TenChuong": string,
    "ThoiGianCapNhat": datetime,
    "LuotXem": number
  }
  ,...
]

````

````console

Method: GET

[1] - URL: hostUrl/api/chuong/{key}

[2] - URL: hostUrl/api/chuong/{id}/{key}

[3] - URL: hostUrl/api/chuong/{id}/{key}/details

````

````console

Method: PUT

URL: hostUrl/api/chuong/{key}
JSON:
{
   "TruyenID": number,
   "TenChuong": string,
   "ThoiGianCapNhat": datetime,
   "LuotXem": number
}

````

````console

Method: DELETE

URL: url/api/chuong/{id}/{key}

````