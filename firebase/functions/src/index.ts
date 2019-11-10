import { region, Request, Response } from 'firebase-functions';


var admin = require("firebase-admin");

var serviceAccount = require("./serviceAccount.json");

admin.initializeApp({
  credential: admin.credential.cert(serviceAccount),
  databaseURL: "https://lookinggrasshackathon2019.firebaseio.com"
});

// // Start writing Firebase Functions
// // https://firebase.google.com/docs/functions/typescript
//
export const getItem = region('asia-northeast1').https.onRequest((request: Request, response: Response) => {
  var db = admin.database();
  db.ref('/items/item').onCreate((snap: any, context: any) => {
    response.json({ state: context });
  });

});

export const listItems = region('asia-northeast1').https.onRequest((request: Request, response: Response) => {
  response.json([{ state: 'success' }]);
});

export const updateItem = region('asia-northeast1').https.onRequest((request: Request, response: Response) => {
  if (request.method !== 'POST') {
    response.send('This is not post request')
  }

  response.send('This is post request')
});
