# TruyenAPI

Rest API was built with ASP.NET Core 3.0 with CRUD operations for a reading book website.
It is using Repository pattern to design architecture and communicating with database by
ORM model.

## Set-up

1. Open your project -> Change your server name in appsetting.json -> Open Window console
2. Typing 'Add-migration create-database' command to create Migrations folder.
3. Typing 'Update-database' to create database in SQL Server.

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
    "TacGiaID": number,
    "TenTruyen": string,
    "MoTa": string,
    "TinhTrang": boolean,
    "HinhAnh": imageURL
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

#### NoiDungChuong table

````console

Method: POST

URL: hostUrl/api/noidungchuong/{key}
JSON:
[
  {
     "ChuongID": number,
     "HinhAnh": imageURL
  }
  ,...
]

````

````console

Method: GET

[1] - URL: hostUrl/api/noidungchuong/{key}

[2] - URL: hostUrl/api/noidungchuong/{id}/{key}

[3] - URL: hostUrl/api/noidungchuong/{id}/{key}/details

````

````console

Method: PUT

URL: hostUrl/api/noidungchuong/{key}
JSON:
{
   "ChuongID": number,
   "HinhAnh": imageURL
}

````

````console

Method: DELETE

URL: url/api/noidungchuong/{id}/{key}

````

#### User table

````console

Method: POST

URL: hostUrl/api/user/{key}
JSON:
{
    "TenUser": string,
    "Password": string
}

````

````console

Method: GET

[1] - URL: hostUrl/api/user/{key}

[2] - URL: hostUrl/api/user/{id}/{key}

[3] - URL: hostUrl/api/user/{id}/{key}/details

````

````console

Method: PUT

URL: hostUrl/api/user/{key}
JSON:
{
    "TenUser": string,
    "Quyen": number,
    "Password": string
}

````

````console

Method: DELETE

URL: url/api/user/{id}/{key}

````

#### TheoDoi table

````console

Method: POST

URL: hostUrl/api/theodoi/{key}
JSON:
{
    "TruyenID": string,
    "Quyen": number,
    "UserID": string
}

````

````console

Method: GET

[1] - URL: hostUrl/api/theodoi/{key}

[2] - URL: hostUrl/api/theodoi/{id}/{key}

[3] - URL: hostUrl/api/theodoi/{id}/{key}/details

````

````console

Method: PUT

URL: hostUrl/api/theodoi/{key}
JSON:
{
    "TruyenID": string,
    "UserID": string
}

````

````console

Method: DELETE

URL: url/api/theodoi/{id}/{key}

````

#### BinhLuan table

````console

Method: POST

URL: hostUrl/api/binhluan/{key}
JSON:
{
    "UserID": string,
    "ChuongID": number,
    "NoiDung": string,
    "NgayBL": datetime
}

````

````console

Method: GET

[1] - URL: hostUrl/api/binhluan/{key}

[2] - URL: hostUrl/api/binhluan/{id}/{key}

[3] - URL: hostUrl/api/binhluan/{id}/{key}/details

````

````console

Method: PUT

URL: hostUrl/api/binhluan/{key}
JSON:
{
    "UserID": string,
    "ChuongID": number,
    "NoiDung": string,
    "NgayBL": datetime
}

````

````console

Method: DELETE

URL: url/api/binhluan/{id}/{key}

````

#### JWT For Login

````console

Method: POST

URL: hostUrl/api/auth/{key}/login
JSON:
{
    "TenUser": string,
    "Password": string
}

````

#### Pagination

1. Truyen table

````console

Method: GET

[Get records]: hostUrl/api/truyen?pageNumber={number}&pageSize={number}&apiKey={key}&getAll=true

[Get lastest updates]: hostUrl/api/truyen?pageNumber={number}&pageSize={number}&apiKey={key}&lastestUpdate=true

[Get top views]: hostUrl/api/truyen?pageNumber={number}&pageSize={number}&apiKey={key}&topView=true

[Get truyen of theloai]: hostUrl/api/truyen?pageNumber={number}&pageSize={number}&apiKey={key}&sorting=true&theloaiID={id}

[Get truyen of theodoi]: hostUrl/api/truyen?pageNumber={number}&pageSize={number}&apiKey={key}&sorting=true&userID={id}

````

2. BinhLuan table

````console

Method: GET

[Get records]: hostUrl/api/binhluan?pageNumber={number}&pageSize={number}&apiKey={key}&getAll=true

[Get lastest updates]: hostUrl/api/binhluan?pageNumber={number}&pageSize={number}&apiKey={key}&lastestUpdate=true

[Get top 10 lastest updates]: hostUrl/api/binhluan?pageNumber={number}&pageSize={number}&apiKey={key}&sorting=true&truyenID={id}

[Get binhluan of chuong]: hostUrl/api/binhluan?pageNumber={number}&pageSize={number}&apiKey={key}&sorting=true&chuongID={id}

````