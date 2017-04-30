from flask import Flask, request
from clarifai.rest import ClarifaiApp
import os

app = Flask(__name__, static_url_path='')


BASE_URL = 'http://6385ca68.ngrok.io'


@app.route('/', methods=['GET', 'POST'])
def index():
	if request.method == 'POST':
		lat = float(request.form['lat'])
		lon = float(request.form['long'])

		resp = str(lat) + ',' + str(lon)

		print resp

		return resp
	else:
		return 'failed'


@app.route('/upload', methods=['GET', 'POST'])
def upload():
	if request.method == 'POST':
		lat = float(request.form['lat'])
		lon = float(request.form['long'])
		file = request.files['image']

		filename = file.filename

		print filename

		file.save(os.path.join('./static', filename))
		
		a = ClarifaiApp("ryx84OAYNB-uaEfUnaWcNSgM_xC9CD10KMjzyrwP", "O3VHCowXj1V9Ex74lVcnvei6ewZvK0QkwN5BvuZo")
		a.auth.get_token()

		model = a.models.get("general-v1.3")
		print model.predict_by_url(url=BASE_URL+'/'+filename)

		return 'success'
	else:
		return 'failed'



@app.route('/<path:path>')
def sound_file(path):
    return url_for('static', filename=path)


if __name__ == '__main__':
	app.run(host='0.0.0.0', port=80, debug=True)