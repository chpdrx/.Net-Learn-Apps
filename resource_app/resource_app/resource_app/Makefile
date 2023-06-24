resource_run:resource_app_build
			docker run -dt -p 8002:80 --name resource_api resource_api:latest

resource_app_build:
	docker build -t resource_api . 