'use strict';
const db = uniCloud.database()
exports.main = async (event, context) => {
  const collection = db.collection('WelfareAPI')
  const res = await collection.get()
  return res
};
