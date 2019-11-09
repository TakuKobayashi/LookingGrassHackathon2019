import { region, Request, Response } from 'firebase-functions';

// // Start writing Firebase Functions
// // https://firebase.google.com/docs/functions/typescript
//
export const sample = region('asia-northeast1').https.onRequest((request: Request, response: Response) => {
  response.json({ state: 'success' });
});
