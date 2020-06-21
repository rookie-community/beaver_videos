'use strict';
const db = uniCloud.database()
exports.main = async (event, context) => {
  const collection = db.collection('UserInfo')
  const res = await collection.where({_id:event._id}).get()
  return res
};
