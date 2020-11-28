<template>
  <div class="imgs">
    <img src="https://site.hubery.wang/imgs/qr/alipay.jpg" alt="支付宝打赏二维码">
    <img src="https://site.hubery.wang/imgs/qr/wxpay.jpg" alt="微信打赏二维码">
  </div>
</template>

<style scoped>
.imgs {
  display: flex;
  flex-direction: row;
  width: 60%;
}

.imgs img {
  width: calc(50% - 10px);
  margin: 10px;
  object-fit: contain;
}

@media (max-width: 768px) {
  .imgs {
    flex-direction: column;
    width: 100%;
  }

  .imgs img {
    width: calc(100% - 20px);
  }
}
</style>
