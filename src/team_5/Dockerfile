FROM python:3.10-slim-bullseye

WORKDIR /app
COPY requirements.txt .
COPY app.py .

RUN pip install -r requirements.txt

ENTRYPOINT ["python", "app.py"]