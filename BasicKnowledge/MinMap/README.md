# MinMap
### Mipmap技术有点类似于LOD技术，但是不同的是，LOD针对的是模型资源，而Mipmap针对的纹理贴图资源。使用Mipmap后，贴图会根据摄像机距离的远近，选择使用不同精度的贴图。
### <font color=red>缺点：</font>会占用内存，因为mipmap会根据摄像机远近不同和贴图的大小（2的N次方）生成对应的N个贴图，所以必然占内存！（本次贴图512*512，为2的9次方，会生成9张不同像素大小的贴图）
### <font color=red>优点：</font>会优化显存带宽，用来减少渲染，因为可以根据实际情况，会选择适合的贴图来渲染，距离摄像机越远，显示的贴图像素越低，反之，像素越高！
### 如果我们使用的贴图不需要这样效果的话，就一定要把Generate Mip Maps选项和Read/Write Enabled选项取消勾选！因为Mipmap会十分占内存！mipMap会让你的包占更大的容量！
### 下面来看下怎么设置贴图的mipmap：
### 设置贴图的Texture Type为Advanced类型 → 勾选Generate Mip Maps → Apply应用
### 应用了minmap的贴图信息：
![微信图片_20190220094656.png](https://i.loli.net/2019/02/20/5c6cb1a670362.png)
### 未应用minmap的贴图信息：
![微信图片_20190220094704.png](https://i.loli.net/2019/02/20/5c6cb1c78052d.png)
### 对应生成的N个不同像素的贴图：
![minmap.gif](https://i.loli.net/2019/02/20/5c6cb2483febd.gif)
### 很明显，应用了minmap后贴图的内存变大了。
### Q1：可否在外部设置贴图的minmap，不通过assetbundle的方式？？？
## （TODO）
### 本文参考借鉴博客： https://www.cnblogs.com/MrZivChu/p/mipmap.html
