
const PROXY_CONFIG = [
  {
    context: [
      "/swagger",
      "/user",
      "/company",
      "/project",
      "/system",
    ],
    target: "https://localhost:7107",
    secure: false
  }
];

module.exports = PROXY_CONFIG;

