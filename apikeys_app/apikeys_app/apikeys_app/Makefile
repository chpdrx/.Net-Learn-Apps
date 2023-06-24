apikeys_run:apikeys_app_build
			docker run -dt -p 8001:80 --name apikey_api apikey_api:latest

apikeys_app_build:
	docker build -t apikey_api . 