import { region, Request, Response } from 'firebase-functions';

// // Start writing Firebase Functions
// // https://firebase.google.com/docs/functions/typescript
//
export const getItem = region('asia-northeast1').https.onRequest((request: Request, response: Response) => {
  response.json({ state: 'success' });
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
