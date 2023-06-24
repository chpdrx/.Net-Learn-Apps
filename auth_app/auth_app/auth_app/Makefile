auth_app_build:
	docker build -t auth_api . 
	
auth_run:auth_app_build
			docker run -dt -p 8000:80 --name auth_api auth_api:latest
