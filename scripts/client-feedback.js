const https = require("https");

const args = process.argv;

const token = args[2];
const issueNumber = args[3];
const title = args[4];
const body = args[5];

(async () => {
  if (!title || !body) return;
  if (title != "From client, DO NOT EDIT!") return;
  const bodyObj = JSON.parse(Buffer.from(body, "base64").toString("utf-8"));
  if (!bodyObj) return;
  console.log("bodyObj", bodyObj);
  const req = https.request(
    {
      hostname: "api.github.com",
      port: 443,
      path: `/repos/hal-wang/Lavcode/issues/${issueNumber}`,
      method: "PATCH",
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: "application/vnd.github+json",
        "user-agent":
          "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36 Edg/105.0.1343.42",
      },
    },
    (res) => {
      let chunks = Buffer.from([]);
      let chunksLength = chunks.length;
      res.on("data", (data) => {
        chunks = Buffer.concat([chunks, data], chunksLength + data.length);
        chunksLength = chunks.length;
      });
      res.on("end", () => {
        console.log("end", chunks.toString("utf-8"));
      });

      console.log("statusCode", res.statusCode);
    }
  );
  req.on("error", (err) => {
    console.log("err", err);
  });
  req.write(
    JSON.stringify({
      title: bodyObj.title,
      body: bodyObj.body,
      labels: [
        "client feedback",
        `platform ${bodyObj.platform}`,
        `version ${bodyObj.version}`,
        `provider ${bodyObj.provider}`,
      ],
      assignees: ["hal-wang"],
    })
  );
  req.end();
})();
